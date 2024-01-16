using AzureSftpBlobSync.JobConfigs.StorageAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders.SftpProvider
{
    public static class Example
    {
        public static Config Get()
        {
            return new Config()
            {
                Name = "SftpAcccount example",
                Id = 2,
                Url = "127.0.0.1",
                Port = 8888,
                UserName = "username",
                Password = "password",
                PrivateKeyPassPhrase = "passphrase",
                PrivateKeyFileName = "/config/privatekey.txt"
            };
        }
    }
}

