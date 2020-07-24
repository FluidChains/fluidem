using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Fluidem.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
// using Microsoft.AspNetCore.Builder;
using Npgsql;
using Options = Fluidem.Core.Models.Options;

namespace Fluidem.Core
{
    public class PostgressFluidem: ICoreFluidem
    {
        private NpgsqlConnection _dbConnection;
        private IOptions<Options> _options;
        
        public PostgressFluidem(IOptions<Options> options)
        {
            _options = options;
            _dbConnection = new NpgsqlConnection(_options.Value.ConnectionString);
            CreateTable();
        }

        public async Task SaveExceptionAsync(DetailError exSave)
        {
            await _dbConnection.ExecuteScalarAsync<int>(
                $@"INSERT INTO {_options.Value.TableName} (id, host, exception_code, status_code, message, stacktrace) 
                        VALUES(@Id, @Host, @ExceptionType, @StatusCode, @Message, @StackTrace)", exSave);
        }

        public void CreateTable()
        {
            using (_dbConnection)
            {
                _dbConnection.Open();
                using (var tblCheck = Commands.CheckTable(_options.Value.TableName))
                {
                    tblCheck.Connection = _dbConnection;
                    var exists = (bool) tblCheck.ExecuteScalar();
                    Console.WriteLine(exists);
                    if (exists) return;
                    using var createAt = Commands.CreateTable(_options.Value.TableName);
                    createAt.Connection = _dbConnection;
                    createAt.ExecuteScalar();
                }
            }
        }

        public DetailError GetException(string uid)
        {
            // GET fo BD Exception
            return new DetailError();
        }
    }
}