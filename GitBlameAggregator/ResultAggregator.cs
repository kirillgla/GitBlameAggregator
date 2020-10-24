using System.IO;
using System.Linq;

namespace GitBlameAggregator
{
    public sealed class ResultAggregator
    {
        private AggregationResult GlobalResult { get; }
        public ResultAggregator() => GlobalResult = new AggregationResult();

        public void RegisterResult(AggregationResult localResult)
        {
            foreach (var (key, value) in localResult.Contributions)
            {
                GlobalResult.RegisterContribution(key, value);
            }
        }

        public void PrintResults(TextWriter writer)
        {
            int maxKeyLength = GlobalResult.Contributions.Keys.Select(key => key.Length).Max();
            foreach (var contribution in GlobalResult.Contributions)
            {
                writer.Write($"{{0,-{maxKeyLength}}}", contribution.Key);
            }
        }
    }
}
