using System;

namespace GitBlameAggregator
{
    public sealed class ProgressReporter
    {
        private int Total { get; }
        private int Current { get; set; }
        private int PreviousPrintedPercentage { get; set; }

        public ProgressReporter(int total)
        {
            Total = total;
            Console.Write("0%");
        }

        public void Advance()
        {
            Current += 1;
            int percentage = (int) (Current * 100.0 / Total);
            if (PreviousPrintedPercentage == percentage) return;
            PreviousPrintedPercentage = percentage;
            ClearCurrentConsoleLine();
            Console.Write($"{percentage}%");
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
