using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace DAL
{
    public class Database : IDatabase
    {
        private readonly SqlConnection sqlConnection;
        public const string LocalDatabase = "Server=localhost;Database=master;User Id=sa;Password=2053Pega;";

        private Database(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
        }
        
        private static Database _instance;

        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Database Instance()
        {
            return _instance ??= new Database(LocalDatabase);
        }


        public SqlTransaction BeginTransaction()
        {
            ConnectionMustBeOpen();
            return sqlConnection.BeginTransaction();
        }

        
        public object ExecuteScalar(string sqlStatement, SqlParameter[] parameters)
        {
            ConnectionMustBeOpen();
            var sqlTransaction = sqlConnection.BeginTransaction();
            var result = ExecuteScalar(sqlStatement, parameters, sqlTransaction);
            sqlTransaction.Commit();
            return result;
        }
        
        public object ExecuteScalar(string sqlStatement, SqlParameter[] parameters, SqlTransaction transaction)
        {
            ConnectionMustBeOpen();
            var command = new SqlCommand(sqlStatement, sqlConnection, transaction);
            command.Parameters.AddRange(parameters);
            return command.ExecuteScalar();
        }

        public void ExecuteNonQuery(string sqlStatement, SqlParameter[] parameters)
        {
            ConnectionMustBeOpen();
            var sqlTransaction = sqlConnection.BeginTransaction();
            ExecuteNonQuery(sqlStatement, parameters, sqlTransaction);
            sqlTransaction.Commit();
        }

        public void ExecuteNonQuery(string sqlStatement, SqlParameter[] parameters, SqlTransaction transaction)
        {
            ConnectionMustBeOpen();
            var command = new SqlCommand(sqlStatement, sqlConnection, transaction);
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
        }
        
        public DataSet ExecuteQuery(string sqlStatement, SqlParameter[] parameters)
        {
            ConnectionMustBeOpen();
            var command = new SqlCommand(sqlStatement, sqlConnection);
            command.Parameters.AddRange(parameters);
            var dataAdapter = new SqlDataAdapter();
            var dataSet = new DataSet();
            
            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(dataSet, "result");
            
            return dataSet;
        }
        
        private void ConnectionMustBeOpen()
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

    }
}