using System;

namespace Arbor.AppModel.Caching
{
    public readonly struct CacheVersion : IComparable<CacheVersion>, IComparable, IEquatable<CacheVersion>
    {
        public bool Equals(CacheVersion other) => Version == other.Version;

        public override bool Equals(object? obj) => obj is CacheVersion other && Equals(other);

        public override int GetHashCode() => Version;

        public static bool operator ==(CacheVersion left, CacheVersion right) => left.Equals(right);

        public static bool operator !=(CacheVersion left, CacheVersion right) => !left.Equals(right);

        public CacheVersion(int version) => Version = version;

        public int Version { get; }

        public int CompareTo(CacheVersion other) => Version.CompareTo(other.Version);

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            return obj is CacheVersion other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(CacheVersion)}");
        }

        public static bool operator <(CacheVersion left, CacheVersion right) => left.CompareTo(right) < 0;

        public static bool operator >(CacheVersion left, CacheVersion right) => left.CompareTo(right) > 0;

        public static bool operator <=(CacheVersion left, CacheVersion right) => left.CompareTo(right) <= 0;

        public static bool operator >=(CacheVersion left, CacheVersion right) => left.CompareTo(right) >= 0;
    }
}