using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitBlameAggregator
{
    public sealed class ExtensionManager
    {
        private static IEnumerable<string> IgnoredExtensions { get; } = new List<string>
        {
            "dll",
            "log",
            "suo",
            "exe",
            "pdb",
            "png",
            "ico",
            "tmp"
        }.Select(it => "." + it).ToList();

        private static IEnumerable<string> KnownExtensions { get; } = new List<string>
        {
            "kt",
            "cs",
            "xml",
            "txt",
            "gold",
            "java",
            "LICENSE",
            "csproj",
            "sln",
            "tt",
            "t4",
            "ttinclude",
            "sh",
            "ps1",
            "md",
            "editorconfig",
            "gitignore",
            "nuke",
            "props",
            "nuspec",
            "config",
            "yml",
            "targets",
            "psi",
            "lex",
            "kts",
            "bnf",
            "cpp",
            "before",
            "after",
            "flex"
        }.Select(it => "." + it).ToList();

        private ISet<string> UnexpectedExtensions { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public bool IsKnownExtension(string extension)
        {
            if (KnownExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase)) return true;
            if (!IgnoredExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                UnexpectedExtensions.Add(extension);
            return false;
        }

        public void ReportUnexpectedExtensions(TextWriter writer)
        {
            foreach (string unexpectedExtension in UnexpectedExtensions)
            {
                writer.WriteLine(unexpectedExtension);
            }

            writer.WriteLine("The extensions above were unexpected and ignored.");
        }
    }
}
