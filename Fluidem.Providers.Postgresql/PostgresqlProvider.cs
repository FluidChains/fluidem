using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Fluidem.Core;
using Fluidem.Core.Models;
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

        public PostgresqlProvider(IDbConnection db, IOptions<FluidemOptions> options,
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
                    exception_type	VARCHAR(100),
                    status_code		VARCHAR(500) NOT NULL,
                    message	        VARCHAR NOT NULL,
                    stacktrace		VARCHAR NOT NULL,
                    time_utc		TIMESTAMP NOT NULL
                );

                ALTER TABLE {tableName} ADD CONSTRAINT PK_{tableName} PRIMARY KEY (id);

                CREATE INDEX IX_{tableName}_Time_Seq ON {tableName} USING BTREE (time_utc DESC);";

            _db.Execute(sql);

            _logger.LogInformation($"Table {tableName} created");
        }

        public async Task SaveExceptionAsync(ErrorDetail exSave)
        {
            await _db.ExecuteScalarAsync<int>(
                $@"INSERT INTO {_options.ErrorLogTableName} (id, host, status_code, message, stacktrace, time_utc) 
                        VALUES(@Id, @Host, @StatusCode, @Message, @StackTrace, @TimeUtc)", exSave);
        }

        public async Task<IEnumerable<ErrorDetail>> GetExceptionsAsync()
        {
            return await _db.QueryAsync<ErrorDetail>(
                $@"SELECT id, host, status_code StatusCode, message, stacktrace, time_utc TimeUtc 
                       FROM {_options.ErrorLogTableName}
                       ORDER BY time_utc DESC");
        }

        public async Task<ErrorDetail> GetExceptionAsync(Guid id)
        {
            return (await _db.QueryAsync<ErrorDetail>(
                $@"SELECT id, host, status_code StatusCode, message, stacktrace, time_utc TimeUtc 
                       FROM {_options.ErrorLogTableName}
                       WHERE id = @id", new {id})).SingleOrDefault();
        }
    }
}