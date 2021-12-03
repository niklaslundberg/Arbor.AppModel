using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arbor.AppModel.Scheduling
{
    public delegate Task OnTickAsync(DateTimeOffset now, CancellationToken cancellationToken);
}