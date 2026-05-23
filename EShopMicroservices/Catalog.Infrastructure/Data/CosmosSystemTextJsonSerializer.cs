using Microsoft.Azure.Cosmos;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    // Cosmos SDK 3.x uses Newtonsoft.Json by default.
    // This serializer replaces it with System.Text.Json
    // so that [JsonPropertyName] attributes on Review are respected.
    public class CosmosSystemTextJsonSerializer : CosmosSerializer
    {
        private readonly JsonSerializerOptions _options;

        public CosmosSystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (stream.CanSeek && stream.Length == 0)
                    return default!;

                return JsonSerializer.Deserialize<T>(stream, _options)!;
            }
        }

        public override Stream ToStream<T>(T input)
        {
            var ms = new MemoryStream();
            JsonSerializer.Serialize(ms, input, _options);
            ms.Position = 0;
            return ms;
        }
    }
}
