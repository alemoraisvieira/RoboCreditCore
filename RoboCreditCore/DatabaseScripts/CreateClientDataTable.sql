CREATE TABLE ClientData (
    RecordId INT PRIMARY KEY IDENTITY(1,1),
    ClientId INT NOT NULL,
    ClientName VARCHAR(100) NOT NULL,
    ClientEmail VARCHAR(100) NOT NULL,
    DataValue  DECIMAL NOT NULL,
    NotificationFlag BIT NOT NULL,
    LastUpdated DATETIME NOT NULL
);

