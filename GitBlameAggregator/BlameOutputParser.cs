using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GitBlameAggregator
{
    public static class BlameOutputParser
    {
        private const string DateTimeRegex = @"\d{4}-\d\d-\d\d \d\d:\d\d:\d\d";

        private static Regex LineRegex { get; } =
            new Regex($@"^[^\(]* \((?<name>.*)\s+{DateTimeRegex} [+-]\d{{4}}\s+\d+\)$", RegexOptions.Compiled);

        public static AggregationResult Parse(string output)
        {
            var result = new AggregationResult();
            foreach (var match in output
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Substring(0, line.IndexOf(')') + 1))
                .Select(metadata => LineRegex.Match(metadata))
            )
            {
                if (!match.Success)
                {
                    Console.WriteLine("ERROR");
                }

                string name = match.Groups["name"].Value.Trim();
                result.RegisterContribution(name);
            }

            return result;
        }
    }
}
