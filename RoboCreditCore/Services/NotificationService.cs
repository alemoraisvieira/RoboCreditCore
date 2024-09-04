using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using RoboCreditCore.Interface;
using RoboCreditCore.Model;
using System;
using System.Threading.Tasks;

namespace RoboCreditCore.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly string _queueName;

        public NotificationService(ServiceBusClient serviceBusClient, ILogger<NotificationService> logger, string queueName)
        {
            _serviceBusClient = serviceBusClient;
            _logger = logger;
            _queueName = queueName;
        }

        public async Task SendNotificationToServiceBus(ClientData clientData)
        {
            try
            {
                ServiceBusSender sender = _serviceBusClient.CreateSender(_queueName);

                ServiceBusMessage message = new ServiceBusMessage
                {
                    Subject = "CreditCore - New Notification",
                    Body = new BinaryData($"User: {clientData.ClientName}, Email: {clientData.ClientEmail}, Data: {clientData.DataValue}"),
                    ApplicationProperties =
                       {
                           { "UserId", clientData.ClientId },
                           { "UserName", clientData.ClientName },
                           { "UserEmail", clientData.ClientEmail },
                           { "DataValue", clientData.DataValue }
                       }
                };

                await sender.SendMessageAsync(message);
                _logger.LogInformation($"Notification sent to {clientData.ClientEmail}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification to Service Bus.");
                throw;
            }
        }
    }
}
