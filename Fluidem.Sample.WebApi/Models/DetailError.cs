using System.Text.Json;

namespace Fluidem.Sample.WebApi.Models
{
    public class DetailError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}