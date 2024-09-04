using System.Data.SqlClient;

namespace RoboCreditCore.Interface
{
    public interface IConnectionHelper
    {
        void SetConnection(string connectionString);
        SqlConnection GetConnection();
        void DisposeConnection();
    }
}
