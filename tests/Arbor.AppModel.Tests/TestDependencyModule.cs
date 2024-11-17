using Arbor.AppModel.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Tests;

[UsedImplicitly]
public class TestDependencyModule(TestDependency testDependency) : IModule
{
    private readonly TestDependency _testDependency = testDependency;

    public IServiceCollection Register(IServiceCollection builder) => builder;
}