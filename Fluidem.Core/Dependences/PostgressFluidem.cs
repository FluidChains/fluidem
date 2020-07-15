using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Fluidem.Core.Models;
using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Builder;
using Npgsql;

namespace Fluidem.Core
{
    public class PostgressFluidem: ICoreFluidem
    {
        private NpgsqlConnection _dbConnection;
        private Options _options;
        
        private PostgressFluidem(Options options)
        {
            _options = options;
            _dbConnection = new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task SaveExceptionAsync(DetailError exSave)
        {
            Console.Write(exSave.ToString());
            await _dbConnection.ExecuteScalarAsync<int>(
                $@"INSERT INTO {_options.TableName} (ErrorId, Host, Type, Message, StatusCode) 
                        VALUES(@Uuid, @Host, @Type, @Message, @StatusCode)", exSave);
        }

        public void CreateTable()
        {
            using (_dbConnection)
            {
                _dbConnection.Open();
                using (var tblCheck = Commands.CheckTable(_options.TableName))
                {
                    tblCheck.Connection = _dbConnection;
                    var exists = (bool) tblCheck.ExecuteScalar();
                    if (exists) return;
                    using var createAt = Commands.CreateTable(_options.TableName);
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