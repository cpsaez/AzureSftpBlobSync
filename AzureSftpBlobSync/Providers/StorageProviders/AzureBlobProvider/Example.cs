using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders.AzureBlobProvider
{
    public static class Example
    {
        public static Config Get()
        {
            return new Config
            {
                ConnectionString = "yourconnectionstring",
                ContainerName = "containername",
                Id = 1,
                Name = "Azure blob storage example"
            };
        }
    }
}
