using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AzureSftpBlobSync.JobConfigs.StorageAccounts;
using AzureSftpBlobSync.Providers.AzureBlobProvider;

namespace AzureSftpBlobSync.JobConfigs
{
    public class AzureSftpBlobSyncConfig
    {
        public AzureSftpBlobSyncConfig()
        {
            PgpKeys = Array.Empty<PgpKey>();
            SftpAccounts = Array.Empty<SftpAccountConfig>();
            BlobAccounts = Array.Empty<Config>();
            JobDefinitions = Array.Empty<JobDefinition>();
        }

        public IEnumerable<SftpAccountConfig> SftpAccounts { get; set; }
        public IEnumerable<PgpKey> PgpKeys { get; set; }
        public IEnumerable<Config> BlobAccounts { get; set; }
        public IEnumerable<JobDefinition> JobDefinitions { get; set; }
    }
}

