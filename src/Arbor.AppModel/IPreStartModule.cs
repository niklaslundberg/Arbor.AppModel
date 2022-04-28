using System.Threading;
using System.Threading.Tasks;

namespace Arbor.AppModel
{
    public interface IPreStartModule
    {
        int Order { get; }

        Task RunAsync(CancellationToken cancellationToken);
    }
}