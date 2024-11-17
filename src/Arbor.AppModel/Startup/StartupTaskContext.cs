using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.AppModel.ExtensionMethods;
using Serilog;

namespace Arbor.AppModel.Startup;

public class StartupTaskContext(IEnumerable<IStartupTask> startupTasks, ILogger logger)
{
    private readonly ImmutableArray<IStartupTask> _startupTasks = startupTasks.SafeToImmutableArray();

    private bool _isCompleted;

    public bool IsCompleted
    {
        get
        {
            if (_isCompleted)
            {
                return true;
            }

            string?[] pendingStartupTasks = _startupTasks.Where(task => !task.IsCompleted)
                .Select(task => task.ToString()).ToArray();

            _isCompleted = pendingStartupTasks.Length == 0;

            if (!_isCompleted)
            {
                logger.Debug("Waiting for startup tasks {Tasks}", string.Join(", ", pendingStartupTasks));
            }
            else if (_startupTasks.Length > 0)
            {
                logger.Debug("All startup tasks are completed");
            }

            return _isCompleted;
        }
    }
}