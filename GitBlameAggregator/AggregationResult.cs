using System.Collections.Generic;

namespace GitBlameAggregator
{
    public sealed class AggregationResult
    {
        private readonly Dictionary<string, int> myContributions;
        public IReadOnlyDictionary<string, int> Contributions => myContributions;
        public AggregationResult() => myContributions = new Dictionary<string, int>();

        public void RegisterContribution(string author, int numberOfLines = 1) =>
            myContributions[author] = myContributions.GetValueOrDefault(author) + numberOfLines;
    }
}
