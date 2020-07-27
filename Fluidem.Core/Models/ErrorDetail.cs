using System;
using System.Text.Json;

namespace Fluidem.Core.Models
{
    public class ErrorDetail
    {
        public Guid Id { get; set; }
        public string Host { get; set; }
        public string ExceptionType { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTimeOffset TimeUtc { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}