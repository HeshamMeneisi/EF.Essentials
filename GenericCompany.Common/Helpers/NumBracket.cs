using System;

namespace GenericCompany.Common.Helpers
{
    public class NumBracket
    {
        public NumBracket(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));

            str = str.Trim();
            if (str.EndsWith('+'))
            {
                InfiniteEnd = true;
                str = str.Trim('+');
            }

            var parts = str.Split('-');
            int start;
            if (int.TryParse(parts[0], out start)) Start = start;
            if (!InfiniteEnd)
            {
                int end;
                if (parts.Length > 1 && int.TryParse(parts[1], out end)) End = end;
            }
            else
            {
                End = int.MaxValue;
            }
        }

        public int? Start { get; }
        public int? End { get; }
        public bool InfiniteEnd { get; }

        public bool Intersects(NumBracket other)
        {
            if (Start == null) return false;
            if (End == null) return Start > other.Start && Start < other.End;
            return other.Start >= Start && other.Start <= End ||
                   other.End >= Start && other.End <= End;
        }
    }
}
