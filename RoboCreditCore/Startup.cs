using RoboCreditCore.Interface;
using RoboCreditCore.Repositories;
using RoboCreditCore.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoboCreditCore.Helper;

[assembly: FunctionsStartup(typeof(RoboCreditCore.Startup))]

namespace RoboCreditCore
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            var sqlConnectionString = configuration["SQLConnectionString"]
                                      ?? throw new InvalidOperationException("SQL connection string not configured.");
            var serviceBusConnectionString = configuration["ServiceBusConnectionString"]
                                             ?? throw new InvalidOperationException("ServiceBus connection string not configured.");
            var queueName = configuration["QueueName"]
                            ?? throw new InvalidOperationException("Queue name not configured.");

            builder.Services.AddSingleton<IConnectionHelper>(sp =>
            {
                var connectionHelper = new ConnectionHelper();
                connectionHelper.SetConnection(sqlConnectionString);
                return connectionHelper;
            });

            builder.Services.AddSingleton<IDataAccessManager>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DataAccessManager>>();
                var connectionHelper = sp.GetRequiredService<IConnectionHelper>();
                return new DataAccessManager(connectionHelper, logger);
            });

            builder.Services.AddSingleton<INotificationService>(sp =>
            {
                var serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
                var logger = sp.GetRequiredService<ILogger<NotificationService>>();
                return new NotificationService(serviceBusClient, logger, queueName);
            });

            builder.Services.AddSingleton(sp =>
                new ServiceBusClient(serviceBusConnectionString));

            builder.Services.AddLogging();
        }
    }
}
