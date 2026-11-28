using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClangSharpSourceToWinmdTests
{
    [TestClass]
    public class NativeCoffAssertionsTests
    {
        [TestMethod]
        public void CodeDataAndLinkage_PreservesPhysicalSectionAndSymbolReordering()
            => NativeCoffAssertions.CodeDataAndLinkageEqual(ObjectFile(), ObjectFile(reorder: true));

        [DataTestMethod]
        [DataRow("payload")]
        [DataRow("relocation")]
        [DataRow("symbol")]
        [DataRow("flags")]
        [DataRow("selection")]
        [DataRow("association")]
        [DataRow("key")]
        [DataRow("checksum32")]
        [DataRow("checksum64")]
        [DataRow("directive")]
        [DataRow("unwind")]
        [DataRow("machine")]
        [DataRow("characteristics")]
        [DataRow("contribution-order")]
        public void CodeDataAndLinkage_RejectsChangedNativeFacts(string mutation)
            => Assert.ThrowsException<AssertFailedException>(() =>
                NativeCoffAssertions.CodeDataAndLinkageEqual(ObjectFile(), ObjectFile(true, mutation)));

        [TestMethod]
        public void CodeDataAndLinkage_RejectsUnknownAuxiliaryRecords()
            => Assert.ThrowsException<InvalidDataException>(() =>
                NativeCoffAssertions.CodeDataAndLinkageEqual(ObjectFile(), ObjectFile(mutation: "unknown-auxiliary")));

        [TestMethod]
        public void CodeDataAndLinkage_PreservesComdatInitializationOrder()
            => Assert.ThrowsException<AssertFailedException>(() =>
                NativeCoffAssertions.CodeDataAndLinkageEqual(ObjectFile(initializers: true),
                    ObjectFile(reorder: true, initializers: true)));

        [TestMethod]
        public void CodeDataAndLinkage_RejectsInvalidComdatDefinitionOrder()
            => Assert.ThrowsException<InvalidDataException>(() =>
                NativeCoffAssertions.CodeDataAndLinkageEqual(ObjectFile(), ObjectFile(mutation: "definition-order")));

        private static byte[] ObjectFile(bool reorder = false, string mutation = "", bool initializers = false)
        {
            var sections = new[]
            {
                new TestSection("first", initializers ? ".CRT$XCU" : ".rdata", new byte[] { 1, 2, 3, 4 }, 0x40301040, 2),
                new TestSection("second", initializers ? ".CRT$XCU" : ".rdata", new byte[] { 5, 6, 7, 8 }, 0x40301040, 2),
                new TestSection("unwind", ".xdata", new byte[] { 9, 10, 11, 12 }, 0x40301040, 5),
                new TestSection("directives", ".drectve", Encoding.ASCII.GetBytes(" /DEFAULTLIB:kernel32.lib"), 0xa00, 0),
                new TestSection("checksums", ".chks64", new byte[40], 0xa00, 0),
            };
            if (mutation == "payload") sections[0].Data[0]++;
            if (mutation == "flags") sections[0].Flags |= 0x80000000;
            if (mutation == "selection") sections[0].Selection = 3;
            if (mutation == "directive") sections[3].Data[2]++;
            if (mutation == "unwind") sections[2].Data[0]++;
            TestSection[] ordered = reorder ?
                new[] { sections[1], sections[2], sections[0], sections[3], sections[4] } : sections;
            if (mutation == "contribution-order")
                (ordered[3], ordered[4]) = (ordered[4], ordered[3]);
            short Number(string id) => checked((short)(Array.FindIndex(ordered, s => s.Id == id) + 1));
            IEnumerable<TestSection> symbolSections = ordered;
            if (reorder)
                symbolSections = symbolSections.Reverse();
            var symbols = symbolSections.SelectMany(s => new[]
            {
                new TestSymbol(s.Name, Number(s.Id), s),
                new TestSymbol("public_" + s.Id, Number(s.Id), null),
            }).Concat(new[]
            {
                new TestSymbol("address", Number(mutation == "symbol" ? "second" : "first"), null),
                new TestSymbol("targetA", 0, null), new TestSymbol("targetB", 0, null),
            }).ToArray();
            if (mutation == "key" || mutation == "definition-order")
            {
                int key = Array.FindIndex(symbols, s => s.Name == "public_first");
                int other = mutation == "key" ? Array.FindIndex(symbols, s => s.Name == "address") :
                    Array.FindIndex(symbols, s => s.Definition?.Id == "first");
                (symbols[key], symbols[other]) = (symbols[other], symbols[key]);
            }
            int symbolCount = 0;
            foreach (var symbol in symbols)
            {
                symbol.Index = symbolCount;
                symbolCount += symbol.Definition == null ? 1 : 2;
            }

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            stream.SetLength(20 + ordered.Length * 40);
            stream.Position = stream.Length;
            foreach (var section in ordered)
            {
                section.DataOffset = checked((int)stream.Position);
                if (section.Id == "checksums")
                {
                    foreach (var target in ordered)
                        writer.Write((ulong)Array.FindIndex(sections, s => s.Id == target.Id) +
                            (mutation == "checksum64" && target.Id == "first" ? 1UL : 0UL));
                }
                else
                {
                    writer.Write(section.Data);
                }

                if (section.Id == "first")
                {
                    section.RelocationOffset = checked((int)stream.Position);
                    writer.Write(0u);
                    writer.Write((uint)symbols.Single(s => s.Name == (mutation == "relocation" ? "targetB" : "targetA")).Index);
                    writer.Write((ushort)3);
                }
            }

            int symbolOffset = checked((int)stream.Position);
            using var strings = new MemoryStream();
            var longNames = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var name in symbols.Select(s => s.Name).Where(s => s.Length > 8).Distinct(StringComparer.Ordinal))
            {
                longNames.Add(name, checked((int)strings.Position + 4));
                strings.Write(Encoding.UTF8.GetBytes(name + "\0"));
            }

            foreach (var symbol in symbols)
            {
                if (longNames.TryGetValue(symbol.Name, out int index))
                {
                    writer.Write(0u);
                    writer.Write((uint)index);
                }
                else
                {
                    Name(symbol.Name);
                }

                var section = symbol.Definition;
                writer.Write(0u);
                writer.Write(symbol.Section);
                writer.Write((ushort)0);
                writer.Write((byte)(section == null ? 2 : 3));
                writer.Write((byte)(section == null ? 0 : mutation == "unknown-auxiliary" ? 2 : 1));
                if (section != null)
                {
                    writer.Write((uint)section.Data.Length);
                    writer.Write((ushort)(section.Id == "first" ? 1 : 0));
                    writer.Write((ushort)0);
                    writer.Write(mutation == "checksum32" && section.Id == "first" ? 1u : 0u);
                    writer.Write(section.Selection == 5 ? Number(mutation == "association" ? "second" : "first") : (short)0);
                    writer.Write(section.Selection);
                    writer.Write((byte)0);
                    writer.Write((ushort)0);
                }
            }

            writer.Write(checked((uint)strings.Length + 4));
            writer.Write(strings.ToArray());
            stream.Position = 0;
            writer.Write((ushort)(mutation == "machine" ? 0xaa64 : 0x8664));
            writer.Write((ushort)ordered.Length);
            writer.Write(reorder ? 27u : 13u);
            writer.Write((uint)symbolOffset);
            writer.Write((uint)symbolCount);
            writer.Write((ushort)0);
            writer.Write((ushort)(mutation == "characteristics" ? 0x100 : 0));
            foreach (var section in ordered)
            {
                Name(section.Name);
                writer.Write(0u);
                writer.Write(0u);
                writer.Write((uint)section.Data.Length);
                writer.Write((uint)section.DataOffset);
                writer.Write((uint)section.RelocationOffset);
                writer.Write(0u);
                writer.Write((ushort)(section.Id == "first" ? 1 : 0));
                writer.Write((ushort)0);
                writer.Write(section.Flags);
            }

            return stream.ToArray();

            void Name(string name)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(name);
                writer.Write(bytes);
                writer.Write(new byte[8 - bytes.Length]);
            }
        }

        private sealed record TestSymbol(string Name, short Section, TestSection? Definition)
        {
            public int Index;
        }

        private sealed record TestSection(string Id, string Name, byte[] Data, uint InitialFlags, byte InitialSelection)
        {
            public uint Flags = InitialFlags;
            public byte Selection = InitialSelection;
            public int DataOffset, RelocationOffset;
        }
    }
}
