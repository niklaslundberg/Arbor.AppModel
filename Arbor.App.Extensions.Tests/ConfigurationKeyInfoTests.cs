using Arbor.App.Extensions.Configuration;
using FluentAssertions;
using Xunit;

namespace Arbor.App.Extensions.Tests
{
    public class ConfigurationKeyInfoTests
    {
        [Theory]
        [InlineData("password")]
        [InlineData("user")]
        [InlineData("secret")]
        [InlineData("connectionString")]
        public void KnownAnonymousKeyShouldBeMadeAnonymous(string key)
        {
            var configurationKeyInfo = new ConfigurationKeyInfo(key, "abc123");

            configurationKeyInfo.Value.Should().Be("*****");
        }
    }
}