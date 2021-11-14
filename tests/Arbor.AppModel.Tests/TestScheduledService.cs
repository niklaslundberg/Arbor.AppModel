﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;
using JetBrains.Annotations;

namespace Arbor.AppModel.Tests
{
    public class TestScheduledService : ScheduledService
    {
        public TestScheduledService([NotNull] ISchedule schedule, [NotNull] IScheduler scheduler) : base(schedule,
            scheduler)
        {
        }

        public int Invokations { get; private set; }

        protected override Task RunAsync(DateTimeOffset currentTime, CancellationToken stoppingToken)
        {
            ++Invokations;

            return Task.CompletedTask;
        }
    }
}