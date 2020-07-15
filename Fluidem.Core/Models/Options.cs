using System;

namespace Fluidem.Core.Models
{
    public class Options
    {
        public string UrlQueryError { get; set; }
        public string LogPath { get; set; }
        public bool SaveHeaders { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
    }
}