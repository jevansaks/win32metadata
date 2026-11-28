using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    internal static class NativeCoffAssertions
    {
        internal static void CodeDataAndLinkageEqual(byte[] expected, byte[] actual)
            => CollectionAssert.AreEqual(Read(expected), Read(actual));

        private static string[] Read(byte[] bytes)
        {
            ReadOnlySpan<byte> Slice(int offset, int count)
            {
                Require(offset >= 0 && count >= 0 && offset <= bytes.Length - count, "Truncated COFF record.");
                return bytes.AsSpan(offset, count);
            }

            ushort U16(int offset) => BinaryPrimitives.ReadUInt16LittleEndian(Slice(offset, 2));
            uint U32(int offset) => BinaryPrimitives.ReadUInt32LittleEndian(Slice(offset, 4));
            int Offset(int offset) => checked((int)U32(offset));
            string Hash(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
            string Text(ReadOnlySpan<byte> value) => new UTF8Encoding(false, true).GetString(value);
            string ShortName(int offset)
            {
                var name = Slice(offset, 8);
                int end = name.IndexOf((byte)0);
                return Text(end < 0 ? name : name.Slice(0, end));
            }

            ushort machine = U16(0), count = U16(2), characteristics = U16(18);
            Require(machine == 0x14c || machine == 0x8664 || machine == 0xaa64, "Unsupported COFF machine.");
            Require(count > 0 && count <= short.MaxValue && U16(16) == 0, "Unsupported COFF header.");
            int symbolOffset = Offset(8), symbolCount = Offset(12);
            int strings = checked(symbolOffset + symbolCount * 18), stringSize = Offset(strings);
            Slice(strings, stringSize);
            string LongName(int index)
            {
                Require(index >= 4 && index < stringSize, "Invalid COFF string index.");
                var tail = Slice(strings + index, stringSize - index);
                int end = tail.IndexOf((byte)0);
                Require(end >= 0, "Unterminated COFF name.");
                return Text(tail.Slice(0, end));
            }

            var sections = new Section[count];
            for (int i = 0; i < count; i++)
            {
                int header = 20 + i * 40;
                string name = ShortName(header);
                if (name.StartsWith("/", StringComparison.Ordinal))
                    name = LongName(int.Parse(name.Substring(1), System.Globalization.CultureInfo.InvariantCulture));
                int size = Offset(header + 16), data = Offset(header + 20);
                var section = sections[i] = new Section
                {
                    Name = name, VirtualSize = U32(header + 8), Address = U32(header + 12),
                    Flags = U32(header + 36), RelocationOffset = Offset(header + 24),
                    RelocationCount = U16(header + 32),
                    Data = data == 0 ? new byte[size] : Slice(data, size).ToArray(),
                };
                Require(data != 0 || size == 0 || (section.Flags & 0x80) != 0, "Missing initialized section data.");
                Require(U32(header + 28) == 0 && U16(header + 34) == 0 &&
                    (section.Flags & 0x1000000) == 0, "COFF line records/relocation overflow are unsupported.");
            }

            var symbols = new Dictionary<int, Symbol>();
            for (int i = 0; i < symbolCount;)
            {
                int offset = checked(symbolOffset + i * 18);
                string name = U32(offset) == 0 ? LongName(Offset(offset + 4)) : ShortName(offset);
                short sectionNumber = unchecked((short)U16(offset + 12));
                ushort type = U16(offset + 14);
                byte storage = Slice(offset + 16, 1)[0], auxiliaries = Slice(offset + 17, 1)[0];
                Require(sectionNumber >= -2 && sectionNumber <= count, "Invalid COFF symbol section.");
                Require(storage == 2 || storage == 3 || storage == 6, "Unsupported COFF symbol storage class.");
                var symbol = new Symbol(name, U32(offset + 8), sectionNumber, type, storage);
                symbols.Add(i, symbol);
                if (sectionNumber > 0)
                    sections[sectionNumber - 1].Symbols.Add(symbol);
                if (auxiliaries != 0)
                {
                    Require(auxiliaries == 1 && storage == 3 && type == 0 && symbol.Value == 0 &&
                        sectionNumber > 0 && name == sections[sectionNumber - 1].Name,
                        "Unsupported COFF auxiliary record.");
                    var section = sections[sectionNumber - 1];
                    Require(section.Auxiliary == null, "Duplicate COFF section definition.");
                    section.DefinitionSymbol = symbol;
                    byte[] auxiliary = Slice(offset + 18, 18).ToArray();
                    section.Auxiliary = auxiliary;
                    section.Selection = auxiliary[14];
                    section.Parent = U16(offset + 30);
                    Require(U32(offset + 18) == section.Data.Length &&
                        U16(offset + 22) == section.RelocationCount && U16(offset + 24) == 0,
                        "Inconsistent COFF section definition.");
                    Require(auxiliary[15] == 0 && U16(offset + 34) == 0 &&
                        section.Selection <= 6, "Unsupported COFF COMDAT auxiliary fields/selection.");
                    Require((section.Selection != 0) == ((section.Flags & 0x1000) != 0),
                        "Inconsistent COFF COMDAT flag.");
                    Require(section.Selection == 5 ? section.Parent > 0 && section.Parent <= count :
                        section.Parent == 0, "Invalid COFF associative section.");
                }

                i = checked(i + 1 + auxiliaries);
                Require(i <= symbolCount, "Truncated COFF auxiliary records.");
            }

            foreach (var section in sections.Where(s => (s.Flags & 0x1000) != 0))
            {
                Require(section.Symbols.Count > 0 && section.DefinitionSymbol != null &&
                    ReferenceEquals(section.Symbols[0], section.DefinitionSymbol),
                    "COMDAT must begin with its static section-definition symbol.");
                if (section.Selection != 5)
                {
                    Require(section.Symbols.Count > 1 && section.Symbols[1].Storage is 2 or 3,
                        "Missing/unsupported COMDAT key symbol.");
                    section.Key = section.Symbols[1].Local;
                }
            }

            string Identity(int index)
            {
                var section = sections[index];
                if (section.Identity != null)
                    return section.Identity;
                Require(!section.Visiting, "Cyclic associative COMDAT.");
                Require((section.Flags & 0x1000) == 0 || section.Auxiliary != null,
                    "Missing COFF COMDAT section definition.");
                Require(section.Selection != 5 || (sections[section.Parent - 1].Flags & 0x1000) != 0,
                    "Associative COFF parent is not COMDAT.");
                section.Visiting = true;
                string parent = section.Selection == 5 ? Identity(section.Parent - 1) : "";
                string data = section.Name == ".chks64" ? "section-indexed checksums" :
                    Convert.ToHexString(SHA256.HashData(section.Data));
                section.Identity = Hash($"{section.Name}|{section.VirtualSize}|{section.Address}|{section.Flags}|{data}|{section.Key}|" +
                    string.Join("|", section.Symbols.Select(s => s.Local).OrderBy(s => s, StringComparer.Ordinal)) + "|" + parent);
                return section.Identity;
            }

            string Binding(Symbol symbol) => symbol.Local + "|" +
                (symbol.Section > 0 ? Identity(symbol.Section - 1) : "special:" + symbol.Section);
            var result = new List<string> { $"header|{machine}|{characteristics}|{count}" };
            for (int i = 0; i < count; i++)
                Identity(i);
            Require(sections.Select(s => s.Identity).Distinct(StringComparer.Ordinal).Count() == count,
                "Ambiguous duplicate COFF section identities.");
            // Only reorder the encountered ordinary COMDAT kinds, never initialization/TLS or unknown contributions.
            result.Add("ordered|" + string.Join("|", sections.Where(s => (s.Flags & 0x1000) == 0 ||
                !(s.Name is ".rdata" or ".data" or ".bss" or ".text$mn" or ".text$di" or ".pdata" or ".xdata"))
                .Select(s => s.Identity)));
            result.AddRange(symbols.Values.Select(Binding).OrderBy(s => s, StringComparer.Ordinal));
            foreach (var section in sections.OrderBy(s => s.Identity, StringComparer.Ordinal))
            {
                string data;
                if (section.Name == ".chks64")
                {
                    // This MSVC table has one checksum per physical section, including itself.
                    Require(section.Flags == 0xa00 && section.Data.Length == count * 8 &&
                        section.RelocationCount == 0, "Unsupported .chks64 format.");
                    data = string.Join("|", sections.Select((s, i) => s.Identity + ":" +
                        BinaryPrimitives.ReadUInt64LittleEndian(section.Data.AsSpan(i * 8, 8)).ToString("X16"))
                        .OrderBy(s => s, StringComparer.Ordinal));
                }
                else
                {
                    data = Convert.ToHexString(section.Data);
                }

                string auxiliary = section.Auxiliary == null ? "none" :
                    Convert.ToHexString(section.Auxiliary.AsSpan(0, 12)) + ":" + section.Selection + ":" +
                    (section.Selection == 5 ? Identity(section.Parent - 1) : "0");
                result.Add($"section|{section.Identity}|{section.Name}|{section.VirtualSize}|{section.Address}|" +
                    $"{section.Flags}|{section.Key}|{section.Data.Length}|{data}|aux:{auxiliary}");
                for (int i = 0; i < section.RelocationCount; i++)
                {
                    int offset = checked(section.RelocationOffset + i * 10);
                    int target = Offset(offset + 4);
                    Require(symbols.ContainsKey(target), "Relocation targets an unknown/auxiliary COFF symbol.");
                    Require(U32(offset) < section.Data.Length, "Relocation lies outside section data.");
                    result.Add($"relocation|{U32(offset)}|{U16(offset + 8)}|{Binding(symbols[target])}");
                }
            }

            return result.ToArray();
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidDataException(message);
        }

        private sealed record Symbol(string Name, uint Value, short Section, ushort Type, byte Storage)
        {
            public string Local => $"{Convert.ToHexString(Encoding.UTF8.GetBytes(Name))}:{Value}:{Type}:{Storage}";
        }

        private sealed class Section
        {
            public string Name = "";
            public uint VirtualSize, Address, Flags;
            public int RelocationOffset, RelocationCount, Parent, Selection;
            public byte[] Data = Array.Empty<byte>();
            public byte[]? Auxiliary;
            public Symbol? DefinitionSymbol;
            public string Key = "";
            public string? Identity;
            public bool Visiting;
            public List<Symbol> Symbols { get; } = new();
        }
    }
}
