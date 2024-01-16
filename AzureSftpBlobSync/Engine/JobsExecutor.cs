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
        private IConfigBuilder config;
        private ILogger<JobsExecutor> logs;

        public JobsExecutor(IConfigBuilder config, ILogger<JobsExecutor> logs)
        {
            this.config = config;
            this.logs = logs;
        }

        public async Task Execute()
        {
            var jsonConfig=await this.config.BuildConfig();
            var jobs = jsonConfig.JobDefinitions;
            var blobsAccounts=jsonConfig.BlobAccounts;
            var sftpAccounts=jsonConfig.SftpAccounts;
            var pgpKeys=jsonConfig.PgpKeys;

            foreach (var job in jobs)
            {
                try
                {
                    var executor = new JobExecutor(job, blobsAccounts, sftpAccounts, pgpKeys, logs);
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
