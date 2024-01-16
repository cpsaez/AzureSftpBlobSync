using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.AzureBlobProvider
{
    public class Config : StorageAccountConfigBase
    {
        public Config()
        {
            ConnectionString = string.Empty;
            ContainerName = string.Empty;
        }
        public override string StorageAccountType => Provider.AzureBlobProvider;
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}
