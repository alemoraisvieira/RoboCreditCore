# Azure Function App for Client Data Processing

## Overview

This Azure Function app processes client data from a SQL Server database and sends notifications based on specified criteria. The app is triggered every 15 minutes to check for new or updated records and sends notifications via Azure Service Bus.

## Features

- **Timer Trigger**: Executes every 15 minutes.
- **Database Interaction**: Retrieves data using SQL Server stored procedures.
- **Notification System**: Sends messages to an Azure Service Bus queue.
- **Dependency Injection**: Utilizes dependency injection for service management.
- **Logging and Error Handling**: Includes robust logging and error management.
- **Unit Testing**: Includes tests for main logic and dependencies.

## Components

### 1. ProcessClientDataFunction
The Azure Function that processes data and sends notifications.

### 2. DataAccessManager
Handles data retrieval from SQL Server.

### 3. ClientData
Models the client data.

### 4. NotificationService
Sends notifications to Azure Service Bus queue.

### 5. ConnectionHelper
Manages SQL Server connections.

### 6. Unit Tests
Unit Tests for functionlities

## Configuration

### Local Development

To configure the application for local development, create or update the `local.settings.json` file in the root of your project with the following content:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "SQLConnectionString": "YourConnectionStringHere",
    "ServiceBusConnectionString": "YourConnectionStringHere",
    "QueueName": "YourQueueNameHere"
  }
}
```
## Configuration Variables:

- **AzureWebJobsStorage:** Use the local Azure Storage emulator for development.
- **FUNCTIONS_WORKER_RUNTIME:** Specifies the runtime for Azure Functions (set to dotnet for C#).
- **SQLConnectionString:** Your connection string to the SQL Server database.
- **ServiceBusConnectionString:** Your connection string for the Azure Service Bus.
- **QueueName:** The name of the Service Bus queue to which messages will be sent.

- ### Azure Development

For deployment on Azure please follow this tutorial:
https://learn.microsoft.com/en-us/azure/azure-functions/functions-develop-vs?pivots=isolated

make sure to configure the environment variables in the Azure Portal,

## Contributing
Contributions are welcome! Please submit a pull request with your changes or improvements.

## Contact
For questions or feedback, please open an issue or contact the repository owner at ale.moraisvieira@gmail.com.

cheers!
