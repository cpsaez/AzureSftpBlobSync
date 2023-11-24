using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.JobConfigs
{
    public class BlobConfig
    {
        public BlobConfig()
        {
            this.ConnectionString = string.Empty;
            this.ContainerName = string.Empty;
        }
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }

        public int Id { get; set; }
    }
}
