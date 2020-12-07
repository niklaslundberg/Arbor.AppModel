using Arbor.App.Extensions.ExtensionMethods;
using FluentAssertions;
using Xunit;

namespace Arbor.App.Extensions.Tests.Enumerables
{
    public class EnumerableOfTests
    {
        private class TestEnumerable
        {
            public static readonly TestEnumerable A = new();
            public static readonly TestEnumerable B = new();
        }

        [Fact]
        public void ClassWithFieldsShouldEnumerateAllInstances() => EnumerableOf<TestEnumerable>.All.Length.Should().Be(2);

        [Fact]
        public void NonEnumClassShouldReturnEmpty() => EnumerableOf<object>.All.IsDefaultOrEmpty.Should().BeTrue();
    }
}