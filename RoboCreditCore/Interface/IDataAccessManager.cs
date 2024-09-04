using RoboCreditCore.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoboCreditCore.Interface
{
    public interface IDataAccessManager
    {
        Task<IEnumerable<ClientData>> GetUpdatedClientDataAsync(DateTime lastExecutionTime);

    }
}
