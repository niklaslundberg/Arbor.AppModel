using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;
using Cronos;
using JetBrains.Annotations;
using Serilog;

namespace Arbor.AppModel.Sample
{
    public class TestSchedule  : ScheduledService
    {
        private readonly ILogger _logger;

        public TestSchedule(ILogger logger) : base(new CronSchedule(CronExpression.Parse("* * * * * *", CronFormat.IncludeSeconds)))
        {
            _logger = logger;
        }

        public override async Task RunAsync(DateTimeOffset currentTime, CancellationToken stoppingToken)
        {
            _logger.Information("Running scheduled task at " + currentTime.ToString("O"));
        }
    }
}
