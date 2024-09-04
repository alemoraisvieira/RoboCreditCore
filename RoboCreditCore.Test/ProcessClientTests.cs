using Microsoft.Extensions.Logging;
using Moq;
using RoboCreditCore.Interface;
using RoboCreditCore.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoboCreditCore;
using Xunit;

namespace RoboCreditCore.Tests
{
    public class ProcessClientTests
    {
        private readonly Mock<IDataAccessManager> _mockDataAccessManager;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<ILogger<ProcessClient>> _mockLogger;
        private readonly ProcessClient _function;

        public ProcessClientTests()
        {
            _mockDataAccessManager = new Mock<IDataAccessManager>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockLogger = new Mock<ILogger<ProcessClient>>();
            _function = new ProcessClient(_mockDataAccessManager.Object, _mockNotificationService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Run_ProcessesClientData()
        {
            // Arrange
            var clientDataList = new List<ClientData>
            {
                new ClientData
                {
                    ClientId = 1, ClientName = "John Doe", ClientEmail = "john@example.com", DataValue = 1000,
                    NotificationFlag = true
                }
            };

            _mockDataAccessManager.Setup(d => d.GetUpdatedClientDataAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(clientDataList);
            _mockNotificationService.Setup(n => n.SendNotificationToServiceBus(It.IsAny<ClientData>()))
                .Returns(Task.CompletedTask);

            // Act
            await _function.Run(null, _mockLogger.Object);

            // Assert
            _mockNotificationService.Verify(n => n.SendNotificationToServiceBus(It.IsAny<ClientData>()), Times.Once);
        }

    }
}