using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Arbor.AppModel.Hosting
{
    public sealed class HostWrapper : IHost
    {
        private readonly IHost _webHostImplementation;

        public HostWrapper(IHost webHost) =>
            _webHostImplementation = webHost ?? throw new ArgumentNullException(nameof(webHost));

        public void Dispose() => _webHostImplementation.Dispose();

        public Task StartAsync(CancellationToken cancellationToken = new()) =>
            _webHostImplementation.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken = new()) =>
            _webHostImplementation.StopAsync(cancellationToken);

        public IServiceProvider Services => _webHostImplementation.Services;

        public void Start() => _webHostImplementation.Start();
    }
}