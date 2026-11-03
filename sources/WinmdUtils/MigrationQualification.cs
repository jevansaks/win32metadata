using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WinmdUtilsProgram
{
    public static class MigrationQualification
    {
        public const string ReferenceSha = "86ed37b2846440297afb087c4718acd1a1fb4ad8";
        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public sealed class BinaryIdentity
        {
            public string Sha256 { get; set; }
            public string NormalizedSha256 { get; set; }
            public string Mvid { get; set; }
            public int MvidOffset { get; set; }
            public int TimestampOffset { get; set; }
            public int Timestamp { get; set; }
        }

        public sealed class BinaryComparison
        {
            public bool Equal { get; set; }
            public int? FirstDifference { get; set; }
            public BinaryIdentity Reference { get; set; }
            public BinaryIdentity Candidate { get; set; }
        }

        public sealed class Verdict
        {
            public bool Passed => this.Errors.Count == 0;
            public List<string> Errors { get; } = new List<string>();
        }

        public static string Hash(byte[] bytes) => Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

        public static BinaryComparison Compare(byte[] first, byte[] second)
        {
            byte[] normalizedFirst = (byte[])first.Clone();
            byte[] normalizedSecond = (byte[])second.Clone();
            var result = new BinaryComparison
            {
                Reference = Normalize(normalizedFirst),
                Candidate = Normalize(normalizedSecond),
                Equal = normalizedFirst.SequenceEqual(normalizedSecond)
            };
            if (!result.Equal)
            {
                int index = 0;
                while (index < Math.Min(normalizedFirst.Length, normalizedSecond.Length) &&
                    normalizedFirst[index] == normalizedSecond[index])
                {
                    index++;
                }

                result.FirstDifference = index;
            }

            return result;
        }

        private static BinaryIdentity Normalize(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes, false);
            using var pe = new PEReader(stream);
            MetadataReader reader = pe.GetMetadataReader(MetadataReaderOptions.None);
            ModuleDefinition module = reader.GetModuleDefinition();
            if (reader.GetTableRowCount(TableIndex.Module) != 1 ||
                module.Generation != 0 || module.Mvid.IsNil || !module.GenerationId.IsNil ||
                !module.BaseGenerationId.IsNil)
            {
                throw new BadImageFormatException("Expected one nonincremental Module with an MVID.");
            }

            Guid mvid = reader.GetGuid(module.Mvid);
            int guidIndex = MetadataTokens.GetHeapOffset(module.Mvid);
            if (guidIndex <= 0 || checked(guidIndex * 16) > reader.GetHeapSize(HeapIndex.Guid))
                throw new BadImageFormatException("MVID index is outside the GUID heap.");
            int mvidOffset = checked(pe.PEHeaders.MetadataStartOffset +
                reader.GetHeapMetadataOffset(HeapIndex.Guid) + (guidIndex - 1) * 16);
            int timestampOffset = checked(pe.PEHeaders.CoffHeaderStartOffset + 4);
            if (mvidOffset < 0 || mvidOffset > bytes.Length - 16 ||
                !bytes.AsSpan(mvidOffset, 16).SequenceEqual(mvid.ToByteArray()) ||
                timestampOffset < 0 || timestampOffset > bytes.Length - 4)
            {
                throw new BadImageFormatException("Metadata reader did not locate the expected MVID/COFF fields.");
            }

            var identity = new BinaryIdentity
            {
                Sha256 = Hash(bytes),
                Mvid = mvid.ToString(),
                MvidOffset = mvidOffset,
                TimestampOffset = timestampOffset,
                Timestamp = pe.PEHeaders.CoffHeader.TimeDateStamp
            };
            // Temporary compatibility with the frozen nondeterministic emitter.
            // No metadata signatures, attributes, import flags or layout are excluded.
            Array.Clear(bytes, mvidOffset, 16);
            Array.Clear(bytes, timestampOffset, 4);
            identity.NormalizedSha256 = Hash(bytes);
            return identity;
        }

        public static Verdict Evaluate(JsonElement receipt)
        {
            var verdict = new Verdict();
            void Require(bool condition, string message)
            {
                if (!condition) verdict.Errors.Add(message);
            }

            string candidateSha = Text(receipt, "candidateSha");
            Require(Text(receipt, "referenceSha") == ReferenceSha, "Reference SHA does not match the frozen snapshot.");
            Require(Matches(candidateSha, 40), "Missing candidate SHA.");
            Require(Text(receipt, "sdkVersion") == "10.0.28000.2705" &&
                Text(receipt, "clangSharpVersion") == "21.1.8.4", "SDK/ClangSharp version mismatch.");
            JsonElement reference = Property(receipt, "reference");
            JsonElement control = Property(receipt, "control");
            JsonElement candidate = Property(receipt, "candidate");
            Require(Text(reference, "sourceSha") == ReferenceSha &&
                Text(control, "sourceSha") == ReferenceSha &&
                Text(candidate, "sourceSha") == candidateSha, "Source receipts do not match the qualified commits.");
            Require(Matches(Text(reference, "inputHash"), 64) &&
                Text(reference, "inputHash") == Text(control, "inputHash"),
                "Frozen reference/control inputs do not match.");
            Require(Matches(Text(candidate, "inputHash"), 64), "Missing current active input hash.");
            Require(Text(reference, "toolSourceSha") == ReferenceSha &&
                Text(control, "toolSourceSha") == candidateSha && Text(candidate, "toolSourceSha") == candidateSha &&
                Matches(Text(reference, "toolHash"), 64) && Matches(Text(control, "toolHash"), 64) &&
                Text(control, "toolHash") == Text(candidate, "toolHash"), "Tool receipts do not match A/B/C.");
            foreach (string name in new[] { "toolchainHash", "sdkHash" })
            {
                Require(Matches(Text(reference, name), 64) && Text(reference, name) == Text(control, name) &&
                    Text(reference, name) == Text(candidate, name),
                    $"Missing/mismatched {name}.");
            }

            JsonElement pairs = Property(receipt, "pairs");
            if (pairs.ValueKind != JsonValueKind.Array || pairs.GetArrayLength() < 1 || pairs.GetArrayLength() > 3)
            {
                verdict.Errors.Add("Expected one to three full paired samples.");
                return verdict;
            }

            var breaches = new Dictionary<string, int>();
            foreach (JsonElement pair in pairs.EnumerateArray())
            {
                bool valid = true;
                foreach (string side in new[] { "reference", "control", "candidate" })
                {
                    JsonElement sample = Property(pair, side);
                    bool sampleValid = Number(sample, "exitCode") == 0 &&
                        Matches(Text(sample, "outputHash"), 64) && Positive(Number(sample, "seconds")) &&
                        Positive(Number(sample, "peakWorkingSetBytes")) && Number(sample, "memorySamples") >= 1 &&
                        DateTimeOffset.TryParse(Text(sample, "startedAt"), out var start) &&
                        DateTimeOffset.TryParse(Text(sample, "endedAt"), out var end) && end >= start;
                    Require(sampleValid, $"Missing/failed generation, measurement or output: {side}.");
                    valid &= sampleValid;
                }

                foreach (var comparison in new[]
                {
                    ("reference", "control", "drift"), ("control", "candidate", "migrationDrift")
                })
                {
                    JsonElement drift = Property(pair, comparison.Item3);
                    Require(IsTrue(Property(drift, "equal")) &&
                        Text(Property(drift, "reference"), "sha256") == Text(Property(pair, comparison.Item1), "outputHash") &&
                        Text(Property(drift, "candidate"), "sha256") == Text(Property(pair, comparison.Item2), "outputHash"),
                        $"Missing, mismatched or differing final metadata: {comparison.Item1}/{comparison.Item2}.");
                }

                // Check the tool regression, migration increment, and cumulative
                // frozen budget independently; B never becomes a ratcheted baseline.
                foreach (var comparison in new[] { ("reference", "control"), ("control", "candidate"), ("reference", "candidate") })
                {
                    foreach (var metric in new[] { ("seconds", 1.10), ("peakWorkingSetBytes", 1.15) })
                    {
                        string key = $"{comparison.Item1}/{comparison.Item2} {metric.Item1}";
                        breaches.TryAdd(key, 0);
                        if (valid && Number(Property(pair, comparison.Item2), metric.Item1) >
                            Number(Property(pair, comparison.Item1), metric.Item1) * metric.Item2)
                            breaches[key]++;
                    }
                }
            }

            foreach (var metric in breaches)
            {
                if (metric.Value > 0 && pairs.GetArrayLength() == 1)
                    verdict.Errors.Add($"{metric.Key} budget needs confirmation.");
                else if (metric.Value > 0 && pairs.GetArrayLength() == 2)
                    verdict.Errors.Add($"{metric.Key} budget breached or unresolved.");
                else if (metric.Value >= 2)
                    verdict.Errors.Add($"Confirmed {metric.Key} budget breach.");
            }

            return verdict;
        }

        public static Verdict VerifyEvidence(string directory, string expectedCandidateSha = null)
        {
            using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(directory, "receipt.json")));
            JsonElement receipt = document.RootElement;
            Verdict verdict = Evaluate(receipt);
            if (!verdict.Passed || !IsTrue(Property(receipt, "passed")))
                throw new InvalidDataException("Qualification failed: " + string.Join("; ", verdict.Errors));
            if (expectedCandidateSha != null && Text(receipt, "candidateSha") != expectedCandidateSha)
                throw new InvalidDataException("Evidence belongs to a different candidate commit.");
            foreach (string side in new[] { "reference", "control", "candidate" })
            {
                VerifyHash($"{side}-inputs.sha256", Text(Property(receipt, side), "inputHash"));
                VerifyHash($"{side}-toolchain.txt", Text(Property(receipt, side), "toolchainHash"));
                VerifyHash($"{side}-sdk.sha256", Text(Property(receipt, side), "sdkHash"));
                VerifyHash($"{side}-tools.sha256", Text(Property(receipt, side), "toolHash"));
            }

            int index = 0;
            foreach (JsonElement pair in Property(receipt, "pairs").EnumerateArray())
            {
                string prefix = $"pair-{++index}";
                foreach (var comparison in new[] { ("reference", "control"), ("control", "candidate") })
                {
                    BinaryComparison drift = Compare(
                        File.ReadAllBytes(Path.Combine(directory, $"{prefix}-{comparison.Item1}.winmd")),
                        File.ReadAllBytes(Path.Combine(directory, $"{prefix}-{comparison.Item2}.winmd")));
                    if (!drift.Equal || drift.Reference.Sha256 != Text(Property(pair, comparison.Item1), "outputHash") ||
                        drift.Candidate.Sha256 != Text(Property(pair, comparison.Item2), "outputHash"))
                        throw new InvalidDataException($"Output binding mismatch: {prefix} {comparison.Item1}/{comparison.Item2}.");
                }

                foreach (string side in new[] { "reference", "control", "candidate" })
                {
                    JsonElement sample = Property(pair, side);
                    VerifyHash($"{prefix}-{side}.binlog", Text(sample, "binlogHash"));
                    VerifyHash($"{prefix}-{side}.log", Text(sample, "logHash"));
                    VerifyHash($"{prefix}-{side}-memory.csv", Text(sample, "memoryLogHash"));
                    using var sampleDocument = JsonDocument.Parse(File.ReadAllText(
                        Path.Combine(directory, $"{prefix}-{side}-sample.json")));
                    foreach (JsonProperty property in sample.EnumerateObject())
                    {
                        if (!JsonElement.DeepEquals(property.Value, Property(sampleDocument.RootElement, property.Name)))
                            throw new InvalidDataException($"Sample binding mismatch: {prefix}-{side} {property.Name}.");
                    }
                }
            }

            return verdict;

            void VerifyHash(string file, string expected)
            {
                byte[] bytes = File.ReadAllBytes(Path.Combine(directory, file));
                if (bytes.Length == 0 || !Matches(expected, 64) || Hash(bytes) != expected)
                    throw new InvalidDataException($"Evidence hash mismatch: {file}.");
            }
        }

        private static JsonElement Property(JsonElement value, string name) =>
            value.ValueKind == JsonValueKind.Object && value.TryGetProperty(name, out var result) ? result : default;
        private static string Text(JsonElement value, string name) =>
            Property(value, name).ValueKind == JsonValueKind.String ? Property(value, name).GetString() : null;
        private static double Number(JsonElement value, string name)
        {
            JsonElement property = Property(value, name);
            return property.ValueKind == JsonValueKind.Number && property.TryGetDouble(out double result)
                ? result : double.NaN;
        }
        private static bool Positive(double value) => double.IsFinite(value) && value > 0;
        private static bool IsTrue(JsonElement value) => value.ValueKind == JsonValueKind.True;
        private static bool Matches(string value, int length) =>
            value != null && Regex.IsMatch(value, $"^[0-9a-f]{{{length}}}$");
    }
}
