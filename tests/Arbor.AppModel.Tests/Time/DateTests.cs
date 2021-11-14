using System;
using System.Collections.Generic;
using Arbor.AppModel.Time;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.AppModel.Tests.Time
{
    public class DateTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public DateTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

        [InlineData(2000, 1, 1)]
        [InlineData(2004, 2, 29)]
        [InlineData(2000, 12, 31)]
        [Theory]
        public void NewDateFromDateTimeShouldReturnInstance(int year, int month, int day)
        {
            var date = new Date(new DateTime(year, month, day));

            date.Year.Should().Be(year);
            date.Month.Should().Be(month);
            date.Day.Should().Be(day);
        }

        [InlineData(2000, 1, 1)]
        [InlineData(2020, 2, 29)]
        [InlineData(2000, 12, 31)]
        [Theory]
        public void DateTryParseValidShouldReturnTrue(int year, int month, int day)
        {
            bool parsed = Date.TryParse(year, month, day, out var date);

            parsed.Should().BeTrue();

            date.Year.Should().Be(year);
            date.Month.Should().Be(month);
            date.Day.Should().Be(day);
        }

        [InlineData(-1, 1, 1)]
        [InlineData(-1, -1, -1)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(int.MinValue, 1, 1)]
        [InlineData(int.MaxValue, 1, 1)]
        [InlineData(10000, 1, 1)]
        [InlineData(2020, 2, 30)]
        [InlineData(2021, 2, 29)]
        [InlineData(2000, 1, 32)]
        [InlineData(2000, 3, 32)]
        [InlineData(2000, 5, 32)]
        [InlineData(2000, 7, 32)]
        [InlineData(2000, 8, 32)]
        [InlineData(2000, 10, 32)]
        [InlineData(2000, 12, 32)]
        [InlineData(2000, 4, 31)]
        [InlineData(2000, 6, 31)]
        [InlineData(2000, 9, 31)]
        [InlineData(2000, 11, 31)]
        [InlineData(2000, 1, int.MaxValue)]
        [InlineData(2000, int.MaxValue, 1)]
        [InlineData(2000, 1, int.MinValue)]
        [InlineData(2000, int.MinValue, 1)]
        [InlineData(2000, 0, int.MaxValue)]
        [InlineData(2000, int.MaxValue, 0)]
        [InlineData(2000, -1, int.MaxValue)]
        [InlineData(2000, int.MaxValue, -1)]
        [InlineData(1, 6, 31)]
        [InlineData(10, 6, 31)]
        [InlineData(100, 6, 31)]
        [InlineData(1000, 6, 31)]
        [Theory]
        public void DateTryParseInvalidShouldReturnFalse(int year, int month, int day)
        {
            bool parsed = Date.TryParse(year, month, day, out _);

            parsed.Should().BeFalse();
        }

        [InlineData(-1, 1, 1)]
        [InlineData(-1, -1, -1)]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(int.MinValue, 1, 1)]
        [InlineData(int.MaxValue, 1, 1)]
        [InlineData(10000, 1, 1)]
        [InlineData(2020, 2, 30)]
        [InlineData(2021, 2, 29)]
        [InlineData(2000, 1, 32)]
        [InlineData(2000, 3, 32)]
        [InlineData(2000, 5, 32)]
        [InlineData(2000, 7, 32)]
        [InlineData(2000, 8, 32)]
        [InlineData(2000, 10, 32)]
        [InlineData(2000, 12, 32)]
        [InlineData(2000, 4, 31)]
        [InlineData(2000, 6, 31)]
        [InlineData(2000, 9, 31)]
        [InlineData(2000, 11, 31)]
        [InlineData(2000, 1, int.MaxValue)]
        [InlineData(2000, int.MaxValue, 1)]
        [InlineData(2000, 1, int.MinValue)]
        [InlineData(2000, int.MinValue, 1)]
        [InlineData(2000, 0, int.MaxValue)]
        [InlineData(2000, int.MaxValue, 0)]
        [InlineData(2000, -1, int.MaxValue)]
        [InlineData(2000, int.MaxValue, -1)]
        [InlineData(1, 6, 31)]
        [InlineData(10, 6, 31)]
        [InlineData(100, 6, 31)]
        [InlineData(1000, 6, 31)]
        [Theory]
        public void NewInvalidDateShouldThrowArgumentException(int year, int month, int day)
        {
            Exception? exception = null;

            try
            {
                _ = new Date(year, month, day);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            _outputHelper.WriteLine(exception?.Message);
            exception.Should().BeOfType<ArgumentException>();
        }

        [Fact]
        public void EqualsOtherAsObjectShouldReturnTrue() =>
            new Date(1, 1, 1).Equals((object)new Date(1, 1, 1)).Should().BeTrue();

        [Fact]
        public void EqualsOtherAsDateShouldReturnTrue() =>
            new Date(1, 1, 1).Equals(new Date(1, 1, 1)).Should().BeTrue();

        [Fact]
        public void EqualsSameDateShouldReturnTrue()
        {
            var date = new Date(1, 1, 1);

            date.Equals(date).Should().BeTrue();
        }

        [Fact]
        public void EqualsSameDateAsObjectShouldReturnTrue()
        {
            var date = new Date(1, 1, 1);

            date.Equals((object)date).Should().BeTrue();
        }

        [Fact]
        public void HashCodeForEqualDateAShouldReturnTrue()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 1);

            date1.GetHashCode().Should().Be(date2.GetHashCode());
        }

        [Fact]
        public void HashCodeForNotEqualDateAShouldReturnFalse()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 2);

            date1.GetHashCode().Should().NotBe(date2.GetHashCode());
        }

        [Fact]
        public void CompareOperatorForGreaterThanShouldReturnFalse()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 1);

            (date1 > date2).Should().BeFalse();
        }

        [Fact]
        public void CompareOperatorForLessThanShouldReturnFalse()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 1);

            (date1 < date2).Should().BeFalse();
        }

        [Fact]
        public void CompareOperatorForGreaterThanShouldReturnTrueForGreater()
        {
            var date1 = new Date(1, 1, 2);
            var date2 = new Date(1, 1, 1);

            (date1 > date2).Should().BeTrue();
        }

        [Fact]
        public void CompareOperatorForLessThanShouldReturnTrueForLess()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 2);

            (date1 < date2).Should().BeTrue();
        }

        [Fact]
        public void CompareOperatorForGreaterThanEqualShouldReturnTrue()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 1);

            (date1 >= date2).Should().BeTrue();
        }

        [Fact]
        public void CompareOperatorForLessThanEqualShouldReturnTrue()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 1);

            (date1 <= date2).Should().BeTrue();
        }

        [Fact]
        public void CompareOperatorForEqualDateShouldReturnTrue()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 1);

            (date1 == date2).Should().BeTrue();
        }

        [Fact]
        public void DatesInDictionaryShouldBeAbleToUseAsKey()
        {
            var date1 = new Date(1, 1, 1);
            var date2 = new Date(1, 1, 2);
            var date3 = new Date(1, 1, 3);

            var dictionary = new Dictionary<Date, Date> { { date2, date2 }, { date1, date1 }, { date3, date3 } };

            dictionary.ContainsKey(new Date(1, 1, 1)).Should().BeTrue();
            dictionary.ContainsKey(new Date(1, 1, 2)).Should().BeTrue();
            dictionary.ContainsKey(new Date(1, 1, 3)).Should().BeTrue();
        }

        [Fact]
        public void EqualsOtherAsDateTimeShouldReturnTrue() =>
            new Date(1, 1, 1).Equals(new DateTime(1, 1, 1)).Should().BeTrue();

        [Fact]
        public void EqualsOtherAsNullShouldReturnFalse() => new Date(1, 1, 1).Equals(null).Should().BeFalse();

        [Fact]
        public void EqualsOtherAsWrongTypeShouldReturnFalse() => new Date(1, 1, 1).Equals(0).Should().BeFalse();

        [Fact]
        public void DefaultShouldReturnIsDefaultTrue() => default(Date).IsDefault.Should().BeTrue();

        [Fact]
        public void NonDefaultShouldReturnIsDefaultFalse() => new Date(1, 1, 1).IsDefault.Should().BeFalse();

        [Fact]
        public void CastFromDateTimeShouldBeEqual() =>
            ((Date)new DateTime(1, 1, 1, 1, 1, 1, DateTimeKind.Utc)).Should().Be(new Date(1, 1, 1));

        [Fact]
        public void CastToDateTimeShouldBeEqual() => ((DateTime)new Date(1, 1, 1)).Should().Be(new DateTime(1, 1, 1));

        [Fact]
        public void CastToDateTimeShouldHaveKindUndefined() =>
            ((DateTime)new Date(1, 1, 1)).Kind.Should().Be(DateTimeKind.Unspecified);
    }
}