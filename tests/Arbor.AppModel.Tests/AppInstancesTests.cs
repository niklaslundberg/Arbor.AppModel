using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Arbor.AppModel.Tests
{
    public class AppInstancesTests
    {
        [Fact]
        public async Task CreateApp()
        {
            object[] instances = [];

            using App<TestStartup> app = await App<TestStartup>.CreateAsync(new CancellationTokenSource(),
                [],
                new Dictionary<string, string>(),
                Array.Empty<Assembly>(),
                instances);

            app.Should().NotBeNull();
        }
    }
}