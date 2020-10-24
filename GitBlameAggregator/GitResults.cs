namespace GitBlameAggregator
{
    public readonly ref struct GitResults
    {
        public int ExitCode { get; }
        public string Output { get; }

        public GitResults(int exitCode, string output)
        {
            ExitCode = exitCode;
            Output = output;
        }
    }
}
