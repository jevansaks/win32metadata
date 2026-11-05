using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetadataUtils
{
    public class EnumWriter : IDisposable
    {
        private string path;
        private string @namespace;
        private string sourceHeaderText;
        private StreamWriter writer;
        private readonly bool useConstants;
        private Dictionary<string, string> namesToValues = new Dictionary<string, string>();

        public EnumWriter(string path, string @namespace, string sourceHeaderText, bool useConstants = true)
        {
            this.path = path;
            this.@namespace = @namespace;
            this.sourceHeaderText = sourceHeaderText;
            this.useConstants = useConstants;
        }

        public bool AddEnum(EnumObject enumObject, IEnumerable<string> associatedConstants = null)
        {
            if (enumObject.name == null || !enumObject.members.Any(m => m.value != null))
            {
                // Skip if we have no name or values to write
                return false;
            }

            this.EnsureStarted();
            this.writer.Write(GetDeclaration(enumObject, associatedConstants));
            return true;
        }

        public static string GetDeclaration(EnumObject enumObject, IEnumerable<string> associatedConstants = null)
        {
            StringWriter writer = new StringWriter();
            if (enumObject.flags)
            {
                writer.WriteLine(
$"    [Flags]");
            }
            foreach (string name in associatedConstants ?? Enumerable.Empty<string>())
                writer.WriteLine($"    [NativeAnnotation(\"Win32Metadata.AssociatedConstant({name})\")]");

            string type = enumObject.type ?? "uint";
            bool forceUnsigned = (enumObject.flags && enumObject.type == null) ||
                enumObject.type is "byte" or "uint" or "ulong" or "ushort";

            StringWriter enumBodyWriter = new StringWriter();
            foreach (var member in enumObject.members.Where(m => m.value != null))
            {
                var currentType = type;
                string valueText = ConstantWriter.FixIntValueText(forceUnsigned, ref currentType, member.value);

                enumBodyWriter.Write(
$@"        {member.name} = {valueText},
");
                // Make the type more specific if it's signed
                if (currentType != type && (type != "int" && type != "long"))
                {
                    type = currentType;
                }
            }

            if (enumObject.type != null)
            {
                type = enumObject.type;
            }

            string typePart = type != "int" ? $" : {type}" : string.Empty;
            writer.WriteLine(
$@"    public enum {enumObject.name}{typePart}
    {{");

            writer.Write(enumBodyWriter.ToString());

            writer.WriteLine(
$@"    }}
");

            return writer.ToString();
        }

        private void EnsureStarted()
        {
            if (this.writer == null)
            {
                this.writer = new StreamWriter(this.path);
                this.writer.WriteLine(
@$"{this.sourceHeaderText}

{(this.useConstants ? $"using static {this.@namespace}.Apis;" : "")}

namespace {this.@namespace}
{{");
            }
        }

        public void Dispose()
        {
            if (this.writer != null)
            {
                this.writer.WriteLine(
"}");
                this.writer.Close();
                this.writer.Dispose();
                this.writer = null;
            }
        }
    }
}
