using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.JobConfigs
{
    public class AzureSftpBlobSyncConfig
    {
        public AzureSftpBlobSyncConfig()
        {
            this.SftpAccounts = string.Empty;
            this.BlobAccounts = string.Empty;
            this.Jobs = string.Empty;
        }

        public string SftpAccounts { get; set; }

        public string BlobAccounts { get; set; }

        public string Jobs { get; set; }

        public IEnumerable<SftpAccountConfig> BuildSftpAccounts()=> this.Parse<SftpAccountConfig>(this.SftpAccounts);
        public IEnumerable<BlobConfig> BuildBlobAccounts()=>this.Parse<BlobConfig>(this.BlobAccounts);
        public IEnumerable<JobDefinition> BuildJobDefinitions() => this.Parse<JobDefinition>(this.Jobs);
       
        private IEnumerable<T> Parse<T>(string variable)
        {
            try
            {
                var result = JsonSerializer.Deserialize<IEnumerable<T>>(this.SftpAccounts);
                if (result == null) { return Enumerable.Empty<T>(); }
                return result;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }
    }
}
