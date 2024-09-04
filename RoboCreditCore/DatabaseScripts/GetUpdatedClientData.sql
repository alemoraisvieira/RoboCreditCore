CREATE PROCEDURE GetUpdatedClientData
    @LastExecutionTime DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        RecordId,
        ClientId,
        ClientName,
        ClientEmail,
        DataValue,
        NotificationFlag
    FROM 
        ClientData
    WHERE 
        LastUpdated > @LastExecutionTime;
END;

