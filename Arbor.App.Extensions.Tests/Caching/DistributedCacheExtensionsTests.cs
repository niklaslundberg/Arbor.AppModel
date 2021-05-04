using System.Net;
using System.Threading.Tasks;
using Arbor.App.Extensions.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Arbor.App.Extensions.Tests.Caching
{
    public class DistributedCacheExtensionsTests
    {
        [Fact]
        public async Task VersionedCached()
        {
            IServiceCollection services = new ServiceCollection().AddDistributedMemoryCache();
            await using var serviceProvider = services.BuildServiceProvider();
            IDistributedCache cache = serviceProvider.GetRequiredService<IDistributedCache>();

            await cache.SetWithVersionAsync("AKey", "test", new CacheVersion(0));

            string? cachedValue = await cache.GetWithVersionAsync<string>("AKey", new CacheVersion(0));

            cachedValue.Should().NotBeNull();

            cachedValue.Should().Be("test");
        }

        [InlineData(0)]
        [InlineData(2)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [Theory]
        public async Task VersionedCachedDifferentVersion(int compareVersion)
        {
            IServiceCollection services = new ServiceCollection().AddDistributedMemoryCache();
            await using var serviceProvider = services.BuildServiceProvider();
            IDistributedCache cache = serviceProvider.GetRequiredService<IDistributedCache>();

            await cache.SetWithVersionAsync("AKey", "test", new CacheVersion(1));

            string? cachedValue = await cache.GetWithVersionAsync<string>("AKey", new CacheVersion(compareVersion));

            cachedValue.Should().BeNull();
        }

        [Fact]
        public async Task VersionedCachedInvalid()
        {
            IServiceCollection services = new ServiceCollection().AddDistributedMemoryCache();
            await using var serviceProvider = services.BuildServiceProvider();
            IDistributedCache cache = serviceProvider.GetRequiredService<IDistributedCache>();

            await cache.SetWithVersionAsync("AKey", "test", new CacheVersion(0));

            var cachedValue = await cache.GetWithVersionAsync<IPAddress>("AKey", new CacheVersion(0));

            cachedValue.Should().BeNull();
        }

        [Fact]
        public async Task VersionedCachedInvalidEmpty()
        {
            IServiceCollection services = new ServiceCollection().AddDistributedMemoryCache();
            await using var serviceProvider = services.BuildServiceProvider();
            IDistributedCache cache = serviceProvider.GetRequiredService<IDistributedCache>();

            await cache.SetStringAsync("AKey", "");

            var cachedValue = await cache.GetWithVersionAsync<IPAddress>("AKey", new CacheVersion(0));

            cachedValue.Should().BeNull();
        }
    }
}