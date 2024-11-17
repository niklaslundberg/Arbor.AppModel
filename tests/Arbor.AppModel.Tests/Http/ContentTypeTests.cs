using FluentAssertions;
using Xunit;

namespace Arbor.AppModel.Tests.Http
{
    public class ContentTypeTests
    {
        [Theory]
        [InlineData("application/json")]
        [InlineData("application/JSON")]
        [InlineData("application/hal+json")]
        public void IsJsonShouldBeTrueForJsonType(string contentType) =>
            ContentType.IsJson(contentType).Should().BeTrue();

        [Theory]
        [InlineData("application/bson")]
        [InlineData("application/xml")]
        [InlineData("text/json")]
        [InlineData("")]
        [InlineData(null!)]
        public void IsJsonShouldBeFalseForNonJsonType(string contentType) =>
            ContentType.IsJson(contentType).Should().BeFalse();

        [Fact]
        public void IsJsonShouldBeTrueForSameReference() =>
            ContentType.IsJson(ContentType.Json.Value).Should().BeTrue();
    }
}