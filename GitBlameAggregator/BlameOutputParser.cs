using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GitBlameAggregator
{
    public static class BlameOutputParser
    {
        private const string DateTimeRegex = @"\d{4}-\d\d-\d\d \d\d:\d\d:\d\d";

        private static Regex LineRegex { get; } =
            new Regex($@"^[0-9a-f]{{9}} \((?<name>.*)\s+{DateTimeRegex} [+-]\d{{4}}\s+\d+\)$", RegexOptions.Compiled);

        public static AggregationResult Parse(string output)
        {
            var result = new AggregationResult();
            foreach (string name in output
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Substring(0, line.IndexOf(')') + 1))
                .Select(metadata => LineRegex.Match(metadata).Groups["name"].Value.Trim()))
            {
                result.RegisterContribution(name);
            }

            return result;
        }
    }
}
