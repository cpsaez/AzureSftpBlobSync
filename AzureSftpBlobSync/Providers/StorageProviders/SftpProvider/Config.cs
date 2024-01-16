using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders.SftpProvider
{
    public class Config : StorageAccountConfigBase
    {
        public Config() : base()
        {
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
        
        public int PrivateKeyId { get; set; }



        public override string StorageAccountType => Provider.SftpProvider;
    }
}
