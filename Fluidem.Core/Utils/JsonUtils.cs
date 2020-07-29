using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fluidem.Core.Utils
{
    public class JsonUtils
    {
        public static async Task<string> SerializeAsync<T>(T obj)
        {
            await using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, obj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static T Deserialize<T>(string jsonText)
        {
            return JsonSerializer.Deserialize<T>(jsonText, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        // public static async Task<T> DeserializeAsync<T>(string jsonText)
        // {
        // await using var stream = new MemoryStream();
        // await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
        // {
        //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        // });
        // stream.Position = 0;
        // var binSerializer = new BinaryFormatter();
        // return (T)binSerializer.Deserialize(stream);
        //}
    }
}