using Microsoft.Extensions.Hosting;

namespace Arbor.AppModel.Startup
{
    public interface IStartupTask : IHostedService
    {
        bool IsCompleted { get; }
    }
}