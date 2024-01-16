using AzureSftpBlobSync.Providers.AzureBlobProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders.SftpProvider
{
    public class Provider : IBlobSyncProvider
    {
        public const string SftpProvider = "SftpProvider";

        public string ProviderName => SftpProvider;

        public StorageAccountConfigBase ConfigurationExample => Example.Get();

        public IProviderStreamer BuilderStreamer(StorageAccountConfigBase config)
        {
            if (config == null) throw new ArgumentNullException("config");
            Config parsedConfig = config as Config ?? throw new Exception($"config is not of expected type fro SftpProvider, is {config.GetType()}");
            SftpWrapper streamer = new SftpWrapper(parsedConfig);
            return streamer;
        }

        public bool CanDeserializeConfig(string storageAccountType)
        {
            if (string.IsNullOrWhiteSpace(storageAccountType)) return false;
            return storageAccountType.Trim().ToLower() == SftpProvider.ToLower();
        }

        public StorageAccountConfigBase DeserializeConfig(string raw)
        {
            var result = JsonSerializer.Deserialize<Config>(raw);
            if (result == null) throw new Exception("The serialization of hte SftpProvider is incorrect");
            return result;
        }
    }
}
