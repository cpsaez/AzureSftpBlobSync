using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders
{
    public interface IBlobSyncProvider
    {
        string ProviderName { get; }

        StorageAccountConfigBase ConfigurationExample { get; }

        bool CanDeserializeConfig(string storageAccountType);

        StorageAccountConfigBase DeserializeConfig(string raw);

        IProviderStreamer BuilderStreamer(StorageAccountConfigBase config);
    }
}
