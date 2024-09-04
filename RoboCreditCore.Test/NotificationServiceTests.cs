using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Moq;
using RoboCreditCore.Model;
using RoboCreditCore.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RoboCreditCore.Tests
{
    public class NotificationServiceTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusSender> _mockServiceBusSender;
        private readonly Mock<ILogger<NotificationService>> _mockLogger;
        private readonly NotificationService _notificationService;
        private const string QueueName = "testQueue";

        public NotificationServiceTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusSender = new Mock<ServiceBusSender>();
            _mockLogger = new Mock<ILogger<NotificationService>>();

            _mockServiceBusClient.Setup(client => client.CreateSender(It.IsAny<string>()))
                .Returns(_mockServiceBusSender.Object);

            _notificationService = new NotificationService(_mockServiceBusClient.Object, _mockLogger.Object, QueueName);
        }

        private ClientData GetClientData()
        {
            return new ClientData
            {
                ClientId = 1,
                ClientName = "John Doe",
                ClientEmail = "john.doe@example.com",
                DataValue = 1000
            };
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldSendNotificationSuccessfully()
        {
            // Arrange
            var clientData = GetClientData();
            ServiceBusMessage sentMessage = null;
            _mockServiceBusSender.Setup(sender => sender.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .Callback<ServiceBusMessage, System.Threading.CancellationToken>((message, _) => sentMessage = message);

            // Act
            await _notificationService.SendNotificationToServiceBus(clientData);

            // Assert
            _mockServiceBusSender.Verify(sender => sender.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default), Times.Once);
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Notification sent to {clientData.ClientEmail}.")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

            Assert.NotNull(sentMessage);
            Assert.Equal("CreditCore - New Notification", sentMessage.Subject);
            Assert.Equal($"User: {clientData.ClientName}, Email: {clientData.ClientEmail}, Data: {clientData.DataValue}", sentMessage.Body.ToString());
            Assert.Equal(clientData.ClientId, sentMessage.ApplicationProperties["UserId"]);
            Assert.Equal(clientData.ClientName, sentMessage.ApplicationProperties["UserName"]);
            Assert.Equal(clientData.ClientEmail, sentMessage.ApplicationProperties["UserEmail"]);
            Assert.Equal(clientData.DataValue, sentMessage.ApplicationProperties["DataValue"]);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldLogError_WhenExceptionThrown()
        {
            // Arrange
            var clientData = GetClientData();

            _mockServiceBusSender.Setup(sender => sender.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .ThrowsAsync(new Exception("exception sending notification to Service Bus"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _notificationService.SendNotificationToServiceBus(clientData));

            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error sending notification to Service Bus.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
