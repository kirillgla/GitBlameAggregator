using System;
using System.Diagnostics;

namespace GitBlameAggregator
{
    public sealed class CommandLine
    {
        private string Root { get; }

        public CommandLine(string root) => Root = root;

        public string Request(string filePath)
        {
            if (IsIgnored(filePath)) return "";
            return RunGit($"-C \"{Root}\" blame \"{filePath}\"").Output;
        }

        private bool IsIgnored(string filePath) =>
            RunGit($"-C \"{Root}\" check-ignore \"{filePath}\"").ExitCode == 0;

        private static GitResults RunGit(string arguments)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Git\cmd\git.exe",
                Arguments = arguments,
                RedirectStandardOutput = true
            });
            if (process == null) throw new NullReferenceException("Could not create a process");
            process.WaitForExit();
            return new GitResults(process.ExitCode, process.StandardOutput.ReadToEnd());
        }
    }
}
