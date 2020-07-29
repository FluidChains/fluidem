using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Fluidem.Core;
using Fluidem.Core.Models;
using Fluidem.Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Fluidem.Providers.Postgresql
{
    public class PostgresqlProvider : IProvider
    {
        private readonly IDbConnection _db;
        private readonly FluidemOptions _options;
        private readonly ILogger<PostgresqlProvider> _logger;

        public PostgresqlProvider(
            IDbConnection db, 
            IOptions<FluidemOptions> options,
            ILogger<PostgresqlProvider> logger)
        {
            if (!(db is NpgsqlConnection))
                throw new Exception("Dbconnection must be an NpgsqlConnection");
            _db = db;
            _options = options.Value;
            _logger = logger;
        }
        
        public void BootstrapProvider()
        {
            var tableName = _options.ErrorLogTableName;
            
            _logger.LogInformation($"Creating {tableName} table");

            var sql =
                $"SELECT 1 FROM information_schema.tables WHERE  table_schema = 'public' AND table_name = '{tableName}'";
            if (_db.Query(sql).Any())
            {
                _logger.LogInformation($"table {tableName} already exists");
                return;
            }

            sql =
                $@"CREATE TABLE {tableName}
                (
                    id		        UUID NOT NULL,
                    host 		    VARCHAR(50) NOT NULL,
                    ""user""        VARCHAR(50) NOT NULL,
                    exception_type	VARCHAR(50) NOT NULL,
                    status_code		VARCHAR(10) NOT NULL,
                    message	        VARCHAR NOT NULL,
                    stacktrace		VARCHAR NOT NULL,
                    time_utc		TIMESTAMP NOT NULL,
                    detail_json     JSON NOT NULL
                );

                ALTER TABLE {tableName} ADD CONSTRAINT PK_{tableName} PRIMARY KEY (id);

                CREATE INDEX IX_{tableName}_Time_Seq ON {tableName} USING BTREE (time_utc DESC);";

            _db.Execute(sql);

            _logger.LogInformation($"Table {tableName} created");
        }

        public async Task SaveExceptionAsync(ErrorDetail ex)
        {
            await _db.ExecuteScalarAsync<int>(
                $@"INSERT INTO {_options.ErrorLogTableName} 
            (id, host, ""user"", exception_type, status_code, message, stacktrace, time_utc, detail_json) 
            VALUES(@Id, @Host, @User, @ExceptionType, @StatusCode, @Message, @StackTrace, @TimeUtc, JSON(@DetailJson))", ex);
        }

        public async Task<IEnumerable<Error>> GetExceptionsAsync()
        {
            return await _db.QueryAsync<Error>(
                $@"SELECT id, host, ""user"", exception_type as ExceptionType, status_code as StatusCode, 
                        message, time_utc TimeUtc 
                       FROM {_options.ErrorLogTableName}
                       ORDER BY time_utc DESC");
        }

        public async Task<ErrorDetail> GetExceptionAsync(Guid id)
        {
            var ex = (await _db.QueryAsync<ErrorDetail>(
                $@"SELECT id, host, ""user"", exception_type as ExceptionType, status_code as StatusCode, 
                        message, stacktrace, time_utc TimeUtc, detail_json as DetailJson 
                       FROM {_options.ErrorLogTableName}
                       WHERE id = @id", new {id})).SingleOrDefault();
            if (ex == null) return null;

            _logger.LogWarning(ex.DetailJson);
            
            ex.Detail = JsonUtils.Deserialize<dynamic>(ex.DetailJson);

            return ex;
        }
    }
}