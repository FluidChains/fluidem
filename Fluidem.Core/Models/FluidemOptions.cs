namespace Fluidem.Core.Models
{
    public class FluidemOptions
    {
        public string ErrorLogApiUrl { get; set; } = "api/error-log";
        public string ErrorLogTableName { get; set; } = "error_log";
        public string ErrorLogFilePath { get; set; }
        public bool SaveHeaders { get; set; }
    }
}