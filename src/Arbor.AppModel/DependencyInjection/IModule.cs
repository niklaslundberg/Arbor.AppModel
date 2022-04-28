using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.DependencyInjection
{
    public interface IModule
    {
        IServiceCollection Register(IServiceCollection builder);
    }
}