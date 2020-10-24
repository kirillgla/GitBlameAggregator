using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitBlameAggregator
{
    internal static class Program
    {
        private static IEnumerable<string> SkippedDirectories { get; } = new List<string>
        {
            ".git", "out", "bin", "artifacts"
        };

        private static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Usage: GitBlameAggregator repository_path");
                return;
            }

            string root = args[0];
            var aggregator = new ResultAggregator();
            var commandLine = new CommandLine(root);
            var manager = new ExtensionManager();
            int count = EnumerateFiles(root, manager).Count();
            manager.ReportUnexpectedExtensions(Console.Out);
            var reporter = new ProgressReporter(count);
            foreach (var path in EnumerateFiles(root, manager))
            {
                string output = commandLine.Request(path);
                var localResult = BlameOutputParser.Parse(output);
                aggregator.RegisterResult(localResult);
                reporter.Advance();
            }

            aggregator.PrintResults(Console.Out);
        }

        private static IEnumerable<string> EnumerateFiles(string root, ExtensionManager manager)
        {
            foreach (string file in Directory.EnumerateFiles(root))
            {
                if (!manager.IsKnownExtension(Path.GetExtension(file))) continue;
                yield return file;
            }

            foreach (string file in Directory
                .EnumerateDirectories(root)
                .Where(directory => !IsSkippedDirectory(directory))
                .SelectMany(directory => EnumerateFiles(directory, manager)))
            {
                yield return file;
            }
        }

        private static bool IsSkippedDirectory(string directory) =>
            SkippedDirectories.Contains(Path.GetFileName(directory));
    }
}
