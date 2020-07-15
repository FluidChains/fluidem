using Npgsql;

namespace Fluidem.Core
{
    public static class Commands
    {
        public static NpgsqlCommand CheckTable(string nameTable)
        {
            var queryCmd = new NpgsqlCommand();
            queryCmd.CommandText = $@"
                                    SELECT EXISTS (
                                       SELECT 1
                                       FROM   information_schema.tables 
                                       WHERE  table_schema = 'public'
                                       AND    table_name = '{nameTable}'
                                       )
                                    ";
            return queryCmd;
        }

        public static NpgsqlCommand CreateTable(string nameTable)
        {
            var queryCmd = new NpgsqlCommand();
            queryCmd.CommandText = $@"CREATE SEQUENCE Fluidem_Error_SEQUENCE;
                                        CREATE TABLE {nameTable}
                                        (
                                            ErrorId		UUID NOT NULL,
                                            Host 		VARCHAR(50) NOT NULL,
                                            Type		VARCHAR(100) NOT NULL,
                                            Message		VARCHAR(500) NOT NULL,
                                            StatusCode	INT NOT NULL,
                                            TimeUtc		TIMESTAMP NOT NULL
                                        );

                                        ALTER TABLE {nameTable} ADD CONSTRAINT PK_FLUIDEM_Error PRIMARY KEY (ErrorId);

                                        CREATE INDEX IX_FLUIDEM_Error_App_Time_Seq ON FLUIDEM_Error USING BTREE
                                        (
                                            TimeUtc       DESC,
                                        );
                                        ";
            return queryCmd;
        }
    }
}