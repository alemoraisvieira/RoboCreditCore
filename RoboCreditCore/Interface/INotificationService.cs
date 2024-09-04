using RoboCreditCore.Model;
using System.Threading.Tasks;

namespace RoboCreditCore.Interface
{
    public interface INotificationService
    {
        Task SendNotificationToServiceBus(ClientData clientData);
    }
}
