using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.App.Extensions.Time
{
    [PublicAPI]
    public struct RelativeInterval : IEquatable<RelativeInterval>
    {
        public bool Equals(RelativeInterval other) =>
            string.Equals(Name, other.Name, StringComparison.Ordinal)
            && FromExclusive == other.FromExclusive
            && ToInclusive == other.ToInclusive;

        public override bool Equals(object? obj) => obj is RelativeInterval other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Name.GetHashCode(StringComparison.InvariantCulture);
                hashCode = (hashCode * 397) ^ FromExclusive;
                hashCode = (hashCode * 397) ^ ToInclusive;
                return hashCode;
            }
        }

        public static bool operator ==(RelativeInterval left, RelativeInterval right) => left.Equals(right);

        public static bool operator !=(RelativeInterval left, RelativeInterval right) => !left.Equals(right);

        public static readonly RelativeInterval Invalid = new RelativeInterval(nameof(Invalid), int.MinValue, -1);

        public static readonly RelativeInterval ThisWeek = new RelativeInterval(nameof(ThisWeek), -1, 7);

        public static readonly RelativeInterval ThisMonth = new RelativeInterval(nameof(ThisMonth), 7, 30);

        public static readonly RelativeInterval ThisQuarter = new RelativeInterval(nameof(ThisQuarter), 30, 90);

        public static readonly RelativeInterval ThisYear = new RelativeInterval(nameof(ThisYear), 90, 365);

        public static readonly RelativeInterval MoreThanAYear = new RelativeInterval(
            nameof(MoreThanAYear),
            365,
            int.MaxValue);

        public RelativeInterval(string name, int fromExclusive, int toInclusive)
        {
            Name = name;
            FromExclusive = fromExclusive;
            ToInclusive = toInclusive;
        }

        public string Name { get; }

        public int FromExclusive { get; }

        public int ToInclusive { get; }

        public static IReadOnlyCollection<RelativeInterval> All => new[]
        {
            Invalid, ThisWeek, ThisMonth, ThisQuarter, ThisYear, MoreThanAYear
        };

        public static RelativeInterval Parse(TimeSpan timeSpan) =>
            All.Single(
                interval => timeSpan.TotalDays > interval.FromExclusive && timeSpan.TotalDays <= interval.ToInclusive);

        public override string ToString()
        {
            if (Equals(Invalid))
            {
                return Constants.NotAvailable;
            }

            return Name;
        }
    }
}