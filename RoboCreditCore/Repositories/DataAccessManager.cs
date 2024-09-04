using Microsoft.Extensions.Logging;
using RoboCreditCore.Interface;
using RoboCreditCore.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RoboCreditCore.Repositories
{
    public class DataAccessManager : IDataAccessManager
    {
        private readonly IConnectionHelper _connectionHelper;
        private readonly ILogger _logger;

        public DataAccessManager(IConnectionHelper connectionHelper, ILogger<DataAccessManager> logger)
        {
            _connectionHelper = connectionHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<ClientData>> GetUpdatedClientDataAsync(DateTime lastExecutionTime)
        {
            var clientDataList = new List<ClientData>();

            try
            {
                await using var sqlCommand = new SqlCommand("GetUpdatedClientData",
                    _connectionHelper.GetConnection());

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.Add(new SqlParameter("@LastExecutionTime", lastExecutionTime));

                await using var reader = await sqlCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var clientData = new ClientData
                    {
                        RecordId = reader.GetInt32(0),
                        ClientId = reader.GetInt32(1),
                        ClientName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        ClientEmail = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        DataValue = reader.GetDecimal(4),
                        NotificationFlag = reader.GetBoolean(5)
                    };
                    clientDataList.Add(clientData);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred while executing the stored procedure GetUpdatedClientData.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in GetUpdatedClientDataAsync method.");
                throw;
            }

            return clientDataList;
        }
    }
}