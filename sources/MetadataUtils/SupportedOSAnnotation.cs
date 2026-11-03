using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MetadataUtils
{
    public static class SupportedOSAnnotation
    {
        public const string Prefix = "Win32Metadata.SupportedOSPlatform";
        private static readonly HashSet<string> Versions = new(StringComparer.Ordinal)
        {
            "windows5.0", "windows5.1.2600", "windows6.0.6000", "windows6.1",
            "windows8.0", "windows8.1", "windows10.0.10240", "windows10.0.10586",
            "windows10.0.14393", "windows10.0.15063", "windows10.0.16299",
            "windows10.0.17134", "windows10.0.17763", "windows10.0.18362",
            "windows10.0.19041", "windowsserver2000", "windowsserver2003",
            "windowsserver2008", "windowsserver2012", "windowsserver2016",
        };

        public static string Parse(string payload)
        {
            var match = Regex.Match(payload, "^Win32Metadata\\.SupportedOSPlatform\\(\"([^\"]+)\"\\)$");
            if (!match.Success || !Versions.Contains(match.Groups[1].Value))
                throw new FormatException($"Malformed or unsupported OS annotation '{payload}'; use a reviewed _Win32_SupportedOS_*_ macro.");
            return match.Groups[1].Value;
        }
    }
}
