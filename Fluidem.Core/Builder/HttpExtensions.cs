using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fluidem.Core.Builder
{
    public static class HttpExtensions
    {
        public static async Task WriteAsJsonAsync<T>(this HttpResponse response, T obj)
        {
            response.ContentType = "application/json";
            await using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, obj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            var jsonText = await reader.ReadToEndAsync();
            await response.WriteAsync(jsonText);
        }
    }
}