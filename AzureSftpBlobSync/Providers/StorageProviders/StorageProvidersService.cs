using AzureSftpBlobSync.Providers.SshProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders
{
    public class StorageProvidersService
    {
        public const string STORAGEPROVIDERSCONFIG = "storages.json";
        private IEnumerable<IBlobSyncProvider> providers;
        private IProviderStreamer configReader;
        private string configFolder;

        public StorageProvidersService(IConfigReader configReader, IEnumerable<IBlobSyncProvider> providers)
        {
            this.providers = providers;
            this.configReader = configReader.GetReader();
            this.configFolder = configReader.GetConfigFolder();
        }

        public async Task<IEnumerable<IBlobSyncProvider>> GetAll()
        {
            var configPath = PathHelper.RemoveBeginSlash(PathHelper.PutEndSlash(this.configFolder) + STORAGEPROVIDERSCONFIG);
            if (!this.configReader.Exists(configPath)) { return Array.Empty<IBlobSyncProvider>(); }

            var raw = await this.configReader.ReadBlob(configPath);
            if (raw == null || raw.Length == 0) { return Array.Empty<IBlobSyncProvider>(); }

            var jsonString = Encoding.UTF8.GetString(raw);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new ProvidersDeserializerConverter(this.providers));


            IEnumerable<StorageAccountConfigBase> configs = new List<StorageAccountConfigBase>();

            try
            {
                var parsedResult = JsonSerializer.Deserialize<IEnumerable<StorageAccountConfigBase>>(jsonString, options);
                if (parsedResult == null)
                {
                    throw new Exception($"The blob file {configPath} cannot be parsed");
                }

                configs = parsedResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"The blob file {configPath} cannot be parsed", ex);
            }
        }
    }
}
