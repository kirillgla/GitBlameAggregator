using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitBlameAggregator
{
    public sealed class ExtensionManager
    {
        private static IEnumerable<string> IgnoredExtensions { get; } = new List<string>
        {
            ".dll"
        };

        private static IEnumerable<string> KnownExtensions { get; } = new List<string>
        {
            ".kt",
            ".cs",
            ".xml",
            ".txt",
            ".gold",
            ".java",
            ".LICENSE",
            ".csproj",
            ".sln",
            ".tt",
            ".t4",
            ".ttinclude",
            ".sh",
            ".ps1",
            ".md",
            ".editorconfig",
            ".gitignore"
        };

        private ISet<string> UnexpectedExtensions { get; } = new HashSet<string>();

        public bool IsKnownExtension(string extension)
        {
            if (KnownExtensions.Contains(extension)) return true;
            if (!IgnoredExtensions.Contains(extension)) UnexpectedExtensions.Add(extension);
            return false;
        }

        public void ReportUnexpectedExtensions(TextWriter writer)
        {
            writer.Write("The following encountered extensions were unexpected and were ignored");
        }
    }
}
