using AzureSftpBlobSync.Providers.StorageProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.SshProvider
{
    public interface IKeysService
    {
        Task<IEnumerable<KeyConfig>> GetKeys();
    }

    public class KeysService : IKeysService
    {
        public const string SSHKEYSCONFIGPATH = "sshkeys.json";
        private IProviderStreamer configReader;
        private string configFolder;
        private IEnumerable<KeyConfig>? keyConfigs = null;

        public KeysService(IConfigReader configReader)
        {
            this.configReader = configReader.GetReader();
            this.configFolder = configReader.GetConfigFolder();

        }

        public async Task<IEnumerable<KeyConfig>> GetKeys()
        {
            if (keyConfigs != null) return keyConfigs;

            var configPath = PathHelper.RemoveBeginSlash(PathHelper.PutEndSlash(this.configFolder) + SSHKEYSCONFIGPATH);

            if (!this.configReader.Exists(configPath)) { return Array.Empty<KeyConfig>(); }

            var raw = await this.configReader.ReadBlob(configPath);
            if (raw == null || raw.Length == 0) { return Array.Empty<KeyConfig>(); }

            var jsonString = Encoding.UTF8.GetString(raw);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            IEnumerable<KeyConfig> result = new List<KeyConfig>();

            try
            {
                var parsedResult = JsonSerializer.Deserialize<IEnumerable<KeyConfig>>(jsonString, options);
                if (parsedResult == null)
                {
                    throw new Exception($"The blob file {configPath} cannot be parsed");
                }

                result = parsedResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"The blob file {configPath} cannot be parsed", ex);
            }

            // we need to fill the content of the PrivateKeys
            foreach (var keyConfig in result)
            {
                if (!string.IsNullOrWhiteSpace(keyConfig.PrivateKeyFilePath))
                {
                    var keyWrapper = await this.configReader.ReadBlob(keyConfig.PrivateKeyFilePath);
                    if (keyWrapper == null) { throw new Exception($"The private key {keyConfig.PrivateKeyFilePath} is null"); }
                    keyConfig.PrivateKey = Encoding.UTF8.GetString(keyWrapper);
                }
            }

            this.keyConfigs = result.ToArray();
            return this.keyConfigs;
        }
    }
}
