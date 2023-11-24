using AzureSftpBlobSync.JobConfigs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Engine
{
    public class JobsExecutor
    {
        private AzureSftpBlobSyncConfig config;

        public JobsExecutor(IOptions<AzureSftpBlobSyncConfig> config)
        {
            this.config = config.Value;
        }  

        public async Task Execute()
        {

        }
    }
}
