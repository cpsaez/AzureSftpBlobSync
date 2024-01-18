using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.StorageProviders
{
    public class ProvidersDeserializerConverter : JsonConverter<StorageAccountConfigBase>
    {
        private IEnumerable<IBlobSyncProvider> providers;

        public ProvidersDeserializerConverter(IEnumerable<IBlobSyncProvider> providers) : base()
        {
            this.providers = providers;
        }
        public override StorageAccountConfigBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                // Obtén el valor del campo "tipo" del JSON
                var root = doc.RootElement;
                if (root.TryGetProperty("StorageAccountType", out var tipoProperty))
                {
                    string? storageAccountType = tipoProperty.GetString();
                    if (storageAccountType == null) { throw new NotSupportedException($"Unknown type: {storageAccountType}"); }
                    if (!string.IsNullOrEmpty(storageAccountType)) { throw new NotSupportedException($"Unknown type: {storageAccountType}"); }

                    var provider = providers.Where(x => x.CanDeserializeConfig(storageAccountType)).FirstOrDefault();
                    if (provider == null) { throw new NotSupportedException($"Unknown type: {storageAccountType}"); }

                    return provider.DeserializeConfig(root.GetRawText());
                }

                throw new JsonException("El campo 'tipo' no está presente en el JSON.");
            }
        }

        public override void Write(Utf8JsonWriter writer, StorageAccountConfigBase value, JsonSerializerOptions options)
        {
            // Implementa la lógica para la serialización aquí si es necesario
            throw new NotImplementedException();
        }
    }
}
