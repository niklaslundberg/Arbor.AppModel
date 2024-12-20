﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.AppModel.Time;

[PublicAPI]
public struct RelativeInterval(string name, int fromExclusive, int toInclusive) : IEquatable<RelativeInterval>
{
    public bool Equals(RelativeInterval other) => string.Equals(Name, other.Name, StringComparison.Ordinal) &&
                                                  FromExclusive == other.FromExclusive &&
                                                  ToInclusive == other.ToInclusive;

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

    public static readonly RelativeInterval Invalid = new(nameof(Invalid), int.MinValue, -1);

    public static readonly RelativeInterval ThisWeek = new(nameof(ThisWeek), -1, 7);

    public static readonly RelativeInterval ThisMonth = new(nameof(ThisMonth), 7, 30);

    public static readonly RelativeInterval ThisQuarter = new(nameof(ThisQuarter), 30, 90);

    public static readonly RelativeInterval ThisYear = new(nameof(ThisYear), 90, 365);

    public static readonly RelativeInterval MoreThanAYear = new(nameof(MoreThanAYear), 365, int.MaxValue);

    public string Name { get; } = name;

    public int FromExclusive { get; } = fromExclusive;

    public int ToInclusive { get; } = toInclusive;

    public static IReadOnlyCollection<RelativeInterval> All =>
        new[] { Invalid, ThisWeek, ThisMonth, ThisQuarter, ThisYear, MoreThanAYear };

    public static RelativeInterval Parse(TimeSpan timeSpan) => All.Single(interval =>
        timeSpan.TotalDays > interval.FromExclusive && timeSpan.TotalDays <= interval.ToInclusive);

    public override string ToString()
    {
        if (Equals(Invalid))
        {
            return Constants.NotAvailable;
        }

        return Name;
    }
}