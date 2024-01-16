using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AzureSftpBlobSync.Providers;

namespace AzureSftpBlobSync.JobConfigs.StorageAccounts
{
    public class SftpAccountConfig : StorageAccountConfigBase
    {
        public SftpAccountConfig():base()
        {
            Name = string.Empty;
            Url = string.Empty;
            Port = 0;
            UserName = string.Empty;
            Password = string.Empty;
            PrivateKey = string.Empty;
            PrivateKeyFileName = string.Empty;
            PrivateKeyPassPhrase = string.Empty;
        }

        public string Url { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string PrivateKey { get; set; }

        public string PrivateKeyFileName { get; set; }

        public string PrivateKeyPassPhrase { get; set; }
    }
}
