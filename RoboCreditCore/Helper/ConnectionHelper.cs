using RoboCreditCore.Interface;
using System.Data.SqlClient;

namespace RoboCreditCore.Helper
{
    public class ConnectionHelper : IConnectionHelper
    {
        private string _connectionString;
        private SqlConnection _connection;

        public void SetConnection(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }

        public void DisposeConnection()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
