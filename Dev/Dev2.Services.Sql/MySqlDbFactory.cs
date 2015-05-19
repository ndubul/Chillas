using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Dev2.Common;
using Dev2.Common.Interfaces.Services.Sql;
using MySql.Data.MySqlClient;

namespace Dev2.Services.Sql
{
    [ExcludeFromCodeCoverage]
    internal class MySqlDbFactory : IDbFactory
    {
        #region Implementation of IDbFactory

        public IDbConnection CreateConnection(string connectionString)
        {
            VerifyArgument.IsNotNull("connectionString", connectionString);

            return new MySqlConnection(connectionString);
        }

        public IDbCommand CreateCommand(IDbConnection connection, CommandType commandType, string commandText)
        {
            return new MySqlCommand(commandText, connection as MySqlConnection)
            {
                CommandType = commandType,
                CommandTimeout = (int) GlobalConstants.TransactionTimeout.TotalSeconds,
            };
        }

        public DataTable GetSchema(IDbConnection connection, string collectionName)
        {

                    return GetMySqlServerSchema(connection);

        }

        DataTable GetMySqlServerSchema(IDbConnection connection)
        {
            if(! (connection is MySqlConnection))
                throw new Exception("Invalid Mqsql Connection");

            return ((MySqlConnection)connection).GetSchema();
            
        }



        public DataTable CreateTable(IDataReader reader, LoadOption overwriteChanges)
        {
            var table = new DataTable();
            table.Load(reader, LoadOption.OverwriteChanges);
            return table;
        }

        public DataSet FetchDataSet(IDbCommand command)
        {
            if (!(command is SqlCommand))
                throw new Exception("Invalid DBCommand expected.");
            using (var dataSet = new DataSet())
            {
                using (var adapter = new SqlDataAdapter(command as SqlCommand))
                {
                    adapter.Fill(dataSet);
                }
                return dataSet;
            }
        }

        #endregion
    }
}