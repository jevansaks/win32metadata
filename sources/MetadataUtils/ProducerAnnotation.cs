using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MetadataUtils
{
    // Internal, compiler-resolved transport; the source annotation has typed Clang arguments.
    public sealed class ProducerAnnotation
    {
        public const string Marker = "Win32Metadata.RAIIFree";
        public string Cleanup { get; }
        public long[] InvalidValues { get; }

        public ProducerAnnotation(string cleanup, long[] invalidValues)
        {
            this.Cleanup = cleanup;
            this.InvalidValues = invalidValues;
        }

        public string Payload => Marker + "(\"" + this.Cleanup + "\"" +
            string.Concat(this.InvalidValues.Select(value => "," + value.ToString(CultureInfo.InvariantCulture))) + ")";

        public static ProducerAnnotation Parse(string payload)
        {
            var match = Regex.Match(payload,
                "\\AWin32Metadata\\.RAIIFree\\(\"([A-Za-z_][A-Za-z_0-9]*(?:::[A-Za-z_][A-Za-z_0-9]*)*)\"((?:,-?[0-9]+)*)\\)\\z");
            if (!match.Success)
                throw new FormatException($"Malformed resolved producer annotation '{payload}'.");
            return new ProducerAnnotation(match.Groups[1].Value, match.Groups[2].Value.Split(',')
                .Skip(1).Select(value => long.Parse(value, CultureInfo.InvariantCulture)).ToArray());
        }
    }
}
