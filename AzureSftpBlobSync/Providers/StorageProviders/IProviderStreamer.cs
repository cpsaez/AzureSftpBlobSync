using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders
{
    public interface IProviderStreamer
    {
        bool Exists(string file);
        Task<byte[]?> ReadBlob(string blobName);
    }
}
