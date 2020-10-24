using System;
using System.Diagnostics;
using System.Text;

namespace GitBlameAggregator
{
    public sealed class CommandLine
    {
        private static readonly char[] Buffer = new char[4096];
        private string Root { get; }
        public CommandLine(string root) => Root = root;

        public string Request(string filePath)
        {
            if (IsIgnored(filePath)) return "";
            return RunGit($"--no-pager -C \"{Root}\" blame \"{filePath}\"").Output;
        }

        private bool IsIgnored(string filePath) =>
            RunGit($"--no-pager -C \"{Root}\" check-ignore \"{filePath}\"").ExitCode == 0;

        private static GitResults RunGit(string arguments)
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Git\cmd\git.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });
            if (process == null) throw new NullReferenceException("Could not create a process");

            string output = process.StandardOutput.ReadToEnd();
            int exitCode = process.ExitCode;
            if (exitCode != 0 && output == string.Empty)
            {
                return new GitResults(exitCode, process.StandardError.ReadToEnd());
            }

            return new GitResults(exitCode, output);
        }
    }
}
