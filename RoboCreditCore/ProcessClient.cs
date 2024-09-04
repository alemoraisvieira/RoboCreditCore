using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RoboCreditCore.Interface;
using System;
using System.Threading.Tasks;

namespace RoboCreditCore
{
    public class ProcessClient
    {
        private readonly IDataAccessManager _dataAccessManager;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        public ProcessClient(
            IDataAccessManager dataAccessManager, 
            INotificationService notificationService, 
            ILogger<ProcessClient> logger = null)
        {
            _dataAccessManager = dataAccessManager;
            _notificationService = notificationService;
            _logger = logger;
        }

        [FunctionName("ProcessClient")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log)
        {
            _logger.LogInformation($"ProcessClient - Function started at: {DateTime.Now}");

            try
            {
                var lastExecutionTime = DateTime.UtcNow.AddMinutes(-15);
                var clientDataList = await _dataAccessManager.GetUpdatedClientDataAsync(lastExecutionTime);
                foreach (var item in clientDataList)
                {
                    if (item.NotificationFlag)
                    {
                        await _notificationService.SendNotificationToServiceBus(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ProcessClient - Error in processing event: {ex.Message}");
            }
        }
    }
}