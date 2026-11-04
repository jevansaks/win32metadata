using System;
using System.Text.RegularExpressions;

namespace MetadataUtils
{
    public static class AssociatedConstantAnnotation
    {
        public const string Prefix = "Win32Metadata.AssociatedConstant";

        public static string Parse(string payload)
        {
            var match = Regex.Match(payload, @"\AWin32Metadata\.AssociatedConstant\(([A-Za-z_][A-Za-z_0-9]*)\)\z");
            if (!match.Success)
                throw new FormatException($"Unknown or malformed owned annotation '{payload}'.");
            return match.Groups[1].Value;
        }
    }
}
