using System;
using AzureSftpBlobSync.Engine;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureSftpBlobSync
{
    public class Scheduler
    {
        private readonly ILogger logger;
        private readonly IJobsExecutor executor;

        public Scheduler(ILoggerFactory loggerFactory, IJobsExecutor executor)
        {
            this.logger = loggerFactory.CreateLogger<Scheduler>();
            this.executor = executor;
        }

        [Function("Scheduler")]
        public async Task Run([TimerTrigger("0 * * * * *")] SchedulerInfo myTimer)
        {
            try
            {
                await executor.Execute();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }
        }
    }

    public class SchedulerInfo
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MyScheduleStatus ScheduleStatus { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
