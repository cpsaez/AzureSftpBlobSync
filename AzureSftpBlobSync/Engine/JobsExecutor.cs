using AzureSftpBlobSync.JobConfigs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AzureSftpBlobSync.Engine
{
    public class JobsExecutor : IJobsExecutor
    {
        private AzureSftpBlobSyncConfig config;
        private ILogger<JobsExecutor> logs;

        public JobsExecutor(IOptions<AzureSftpBlobSyncConfig> config, ILogger<JobsExecutor> logs)
        {
            this.config = config.Value;
            this.logs = logs;
        }

        public async Task Execute()
        {
            var jobs = this.config.BuildJobDefinitions();
            var blobsAccounts=this.config.BuildBlobAccounts();
            var sftpAccounts=this.config.BuildSftpAccounts(); 

            foreach (var job in jobs)
            {
                try
                {
                    var executor = new JobExecutor(job, blobsAccounts, sftpAccounts);
                    await executor.Execute();
                }
                catch (Exception ex)
                {
                    this.logs.LogError(ex, $"Error executing job {job.Name}");
                }
            }
        }
    }
}
