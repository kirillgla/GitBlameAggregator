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
            ".git", "out", "bin"
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
            foreach (var path in EnumerateFiles(root))
            {
                string output = commandLine.Request(path);
                var localResult = BlameOutputParser.Parse(output);
                aggregator.RegisterResult(localResult);
            }

            aggregator.PrintResults(Console.Out);
        }

        private static IEnumerable<string> EnumerateFiles(string root)
        {
            foreach (string file in Directory.EnumerateFiles(root))
            {
                yield return file;
            }

            foreach (string file in Directory
                .EnumerateDirectories(root)
                .Where(IsNotSkipped)
                .SelectMany(EnumerateFiles))
            {
                yield return file;
            }
        }

        private static bool IsNotSkipped(string directory)
        {
            string? name = Path.GetDirectoryName(directory);
            if (name == null) return false;
            return !SkippedDirectories.Contains(directory);
        }
    }
}
