using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Fluidem.Core;
using Fluidem.Core.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Fluidem.Providers.Postgresql
{
    public class PostpresqlProvider : IProvider
    {
        private readonly IDbConnection _db;
        private readonly IOptions<FluidemOptions> _options;

        public PostpresqlProvider(IDbConnection db, IOptions<FluidemOptions> options)
        {
            if (!(db is NpgsqlConnection))
                throw new Exception("Dbconnection must be an NpgsqlConnection");
            _db = db;
            _options = options;
        }

        // public void BootstrapProvider()
        // {
        //     
        //         using (var tblCheck = Commands.CheckTable(_options.Value.TableName))
        //         {
        //             tblCheck.Connection = _db;
        //             var exists = (bool) tblCheck.ExecuteScalar();
        //             Console.WriteLine(exists);
        //             if (exists) return;
        //             using var createAt = Commands.CreateTable(_options.Value.TableName);
        //             createAt.Connection = _db;
        //             createAt.ExecuteScalar();
        //         }
        //     
        // }

        public void BootstrapProvider()
        {
            var sql =
                $@"CREATE SEQUENCE {_options.Value.TableName}_SEQUENCE;
                    CREATE TABLE IF NOT EXISTS {_options.Value.TableName}
                    (
                        id		        UUID NOT NULL,
                        host 		    VARCHAR(50) NOT NULL,
                        exception_type	VARCHAR(100) NOT NULL,
                        status_code		VARCHAR(500) NOT NULL,
                        message	        INT NOT NULL,
                        stacktrace		VARCHAR(500) NOT NULL,
                        timeUtc		    TIMESTAMP NOT NULL,
                    );

                    ALTER TABLE {_options.Value.TableName} ADD CONSTRAINT PK_{_options.Value.TableName} PRIMARY KEY (id);

                    CREATE INDEX IX_FLUIDEM_Error_App_Time_Seq ON {_options.Value.TableName} USING BTREE
                    (
                        TimeUtc       DESC,
                    );";

            _db.Execute(sql);
        }

        public async Task SaveExceptionAsync(DetailError exSave)
        {
            await _db.ExecuteScalarAsync<int>(
                $@"INSERT INTO {_options.Value.TableName} (id, host, exception_code, status_code, message, stacktrace) 
                        VALUES(@Id, @Host, @ExceptionType, @StatusCode, @Message, @StackTrace)", exSave);
        }

        public async Task<DetailError> GetExceptionAsync(string uid)
        {
            return (await _db.QueryAsync<DetailError>(
                $@"SELECT id, host, status_code StatusCode, message, stacktrace 
                       FROM {_options.Value.TableName}")).SingleOrDefault();
        }
    }
}