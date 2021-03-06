using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Fluidem.Core.Utils;

namespace Fluidem.Core.Models
{
    public class Error
    {
        public Guid Id { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string ExceptionType { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTimeOffset TimeUtc { get; set; }
    }

    public class ErrorDetail
    {
        public Guid Id { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string ExceptionType { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public DateTimeOffset TimeUtc { get; set; }
        [JsonIgnore] public string DetailJson { get; set; }
        public dynamic Detail { get; set; }
    }
}