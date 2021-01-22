using System;
using System.Collections.Generic;
using Arbor.App.Extensions.Cli;
using FluentAssertions;
using Xunit;

namespace Arbor.App.Extensions.Tests.Cli
{
    public class ParameterParserTests
    {
        [Fact]
        public void ParseValidParameterShouldReturnValue()
        {
            IReadOnlyCollection<string> parameters = new List<string> {"test=123"};
            string? value = parameters.ParseParameter("test");

            value.Should().Be("123");
        }

        [Fact]
        public void ParseEmptyParameterShouldReturnEmptyString()
        {
            IReadOnlyCollection<string> parameters = new List<string> {"test="};
            string? value = parameters.ParseParameter("test");

            value.Should().BeEmpty();
        }

        [Fact]
        public void ParseMissingParameterShouldReturnNull()
        {
            IReadOnlyCollection<string> parameters = new List<string> {"test="};
            string? value = parameters.ParseParameter("non-existing");

            value.Should().BeNull();
        }

        [Fact]
        public void ParseInvalidParameterShouldReturnNull()
        {
            IReadOnlyCollection<string> parameters = new List<string> {"test"};
            string? value = parameters.ParseParameter("test");

            value.Should().BeNull();
        }

        [Fact]
        public void ParseDuplicateParameterShouldThrowInvalidOperationException()
        {
            IReadOnlyCollection<string> parameters = new List<string> {"test=123", "test=234"};
            Func<string?> function = () => parameters.ParseParameter("test");

            function.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void InvalidParameterNameShouldThrowArgumentNullException()
        {
            IReadOnlyCollection<string> parameters = new List<string> {"test="};
            Func<string?> function = () => parameters.ParseParameter(null!);

            function.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void InvalidCollectionShouldThrowArgumentNullException()
        {
            IReadOnlyCollection<string> parameters = null!;
            Func<string?> function = () => parameters.ParseParameter("anything");

            function.Should().Throw<ArgumentNullException>();
        }
    }
}