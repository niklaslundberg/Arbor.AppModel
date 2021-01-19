using System;
using System.Globalization;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Arbor.App.Extensions.Time
{
    public readonly struct Date : IEquatable<Date>, IComparable<Date>
    {
        [PublicAPI]
        [JsonIgnore]
        public DateTime OriginalValue { get; }

        private readonly DateTime _datePart;

        public Date(DateTime date)
        {
            OriginalValue = date;
            _datePart = date.Date;
            Year = _datePart.Year;
            Month = _datePart.Month;
            Day = _datePart.Day;
        }

        public Date(int year, int month, int day) : this(IsValid(year, month, day)
            ? new DateTime(year, month, day)
            : throw new ArgumentException(CreateMessage(year, month, day)))
        {
        }

        private static string CreateMessage(int year, int month, int day)
        {
            if (year < 1 || year > 9999)
            {
                return $"Year must be in interval 0001-9999, was {year.ToString(CultureInfo.InvariantCulture)}";
            }

            if (month < 1 || month > 12)
            {
                return $"Month must be in interval 01-12, was {month.ToString(CultureInfo.InvariantCulture)}";
            }

            if (day < 1 || day > 31)
            {
                return $"Day must be in interval 01-31, was {day.ToString(CultureInfo.InvariantCulture)}";
            }

            return $"Invalid date {PrefixYear(year)}-{PrefixDayOrMonth(month)}-{PrefixDayOrMonth(day)}";
        }

        private static string PrefixYear(int year)
        {
            string invariantYear = year.ToString(CultureInfo.InvariantCulture);

            return year switch
            {
                >= 1 and < 10 => $"000{invariantYear}",
                >= 1 and < 100 => $"00{invariantYear}",
                >= 1 and < 1000 => $"0{invariantYear}",
                _ => invariantYear
            };
        }

        private static string PrefixDayOrMonth(int dayOrMonth)
        {
            string invariant = dayOrMonth.ToString(CultureInfo.InvariantCulture);

            if (dayOrMonth >= 0 && dayOrMonth < 10)
            {
                return $"0{invariant}";
            }

            return invariant;
        }

        public bool IsDefault => Year == 0 && Month == 0 && Day == 0;

        public int Year { get; }

        public int Month { get; }

        public int Day { get; }

        public override string ToString() => _datePart.ToString("yyyy-MM-dd");

        public static explicit operator Date(DateTime dateTime) => new(dateTime);

        public static explicit operator DateTime(Date date)
        {
            if (date.IsDefault)
            {
                throw new InvalidCastException("The source date is default");
            }

            return new DateTime(date.Year, date.Month, date.Day, 0,
                0, 0, DateTimeKind.Unspecified);
        }

        public bool Equals(Date other) => _datePart.Equals(other._datePart);

        public override bool Equals(object? obj) =>
            obj switch
            {
                Date other => Equals(other),
                DateTime asDateTime => Equals((Date)asDateTime),
                _ => false
            };

        public override int GetHashCode() => _datePart.GetHashCode();

        public static bool operator ==(Date left, Date right) => left.Equals(right);

        public static bool operator !=(Date left, Date right) => !left.Equals(right);

        public int CompareTo(Date other) => _datePart.CompareTo(other._datePart);

        public static bool operator <(Date date1, Date date2) => date1._datePart < date2._datePart;

        public static bool operator >(Date date1, Date date2) => date1._datePart > date2._datePart;

        public static bool operator >=(Date date1, Date date2) => date1._datePart >= date2._datePart;

        public static bool operator <=(Date date1, Date date2) => date1._datePart <= date2._datePart;

        private static bool IsValid(in int year, in int month, in int day)
        {
            if (year < 1 || year > 9999)
            {
                return false;
            }

            if (month < 1 || month > 12)
            {
                return false;
            }

            if (day < 1 || day > 31)
            {
                return false;
            }

            bool isLeapYear = DateTime.IsLeapYear(year);

            bool is30DayMonth = month switch
            {
                4 => true,
                6 => true,
                9 => true,
                11 => true,
                _ => false
            };

            bool is31DayMonth = !is30DayMonth && month != 2;

            return month switch
            {
                2 when isLeapYear && day <= 29 => true,
                2 when !isLeapYear && day <= 28 => true,
                _ when is30DayMonth && day <= 30 => true,
                _ when is31DayMonth && day <= 31 => true,
                _ => false
            };
        }

        public static bool TryParse(in int year, in int month, in int day, out Date date)
        {
            bool isValid = IsValid(year, month, day);

            date = isValid
                ? new Date(year, month, day)
                : default;

            return isValid;
        }
    }
}