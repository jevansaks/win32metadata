using System;
using System.Text.RegularExpressions;

namespace MetadataUtils
{
    public enum ParameterAnnotationKind { Retval, Retained, ContainsInteriorPointers, AssociatedEnum }

    public readonly struct ParameterAnnotation
    {
        public const string Owner = "Win32Metadata.";
        public ParameterAnnotationKind Kind { get; }
        public string EnumName { get; }

        public ParameterAnnotation(ParameterAnnotationKind kind, string enumName = null)
        {
            this.Kind = kind;
            this.EnumName = enumName;
        }

        public static ParameterAnnotation Parse(string payload)
        {
            switch (payload)
            {
                case Owner + "Retval": return new(ParameterAnnotationKind.Retval);
                case Owner + "Retained": return new(ParameterAnnotationKind.Retained);
                case Owner + "ContainsInteriorPointers": return new(ParameterAnnotationKind.ContainsInteriorPointers);
            }
            var match = Regex.Match(payload, @"\AWin32Metadata\.AssociatedEnum\(([A-Za-z_][A-Za-z0-9_]*)\)\z");
            if (match.Success) return new(ParameterAnnotationKind.AssociatedEnum, match.Groups[1].Value);
            throw new FormatException($"Unknown or malformed owned annotation '{payload}'.");
        }

        public string AttributeName => Kind == ParameterAnnotationKind.Retval ? "RetVal" : Kind.ToString();
    }
}
