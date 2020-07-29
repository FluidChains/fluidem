using System.Threading.Tasks;
using Fluidem.Core.Utils;
using Microsoft.AspNetCore.Http;

namespace Fluidem.Core.Builder
{
    public static class HttpExtensions
    {
        public static async Task WriteAsJsonAsync<T>(this HttpResponse response, T obj)
        {
            response.ContentType = "application/json";
            var jsonText = await JsonUtils.SerializeAsync(obj);
            await response.WriteAsync(jsonText);
        }
    }
}