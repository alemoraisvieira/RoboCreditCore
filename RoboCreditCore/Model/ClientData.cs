namespace RoboCreditCore.Model
{
    public class ClientData
    {
        public int RecordId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public decimal DataValue { get; set; }
        public bool NotificationFlag { get; set; }

    }
}
