using System;
using System.Threading.Tasks;

namespace Arbor.AppModel.Scheduling
{
    public delegate Task OnTickAsync(DateTimeOffset now);
}