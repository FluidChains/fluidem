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
                                            id		        UUID NOT NULL,
                                            host 		    VARCHAR(50) NOT NULL,
                                            exception_type	VARCHAR(100) NOT NULL,
                                            status_code		VARCHAR(500) NOT NULL,
                                            message	        INT NOT NULL,
                                            stacktrace		VARCHAR(500) NOT NULL,
                                            timeUtc		    TIMESTAMP NOT NULL,
                                        );

                                        ALTER TABLE {nameTable} ADD CONSTRAINT PK_FLUIDEM_Error PRIMARY KEY (id);

                                        CREATE INDEX IX_FLUIDEM_Error_App_Time_Seq ON {nameTable} USING BTREE
                                        (
                                            TimeUtc       DESC,
                                        );
                                        ";
            return queryCmd;
        }
    }
}