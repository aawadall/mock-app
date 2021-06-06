namespace Logger
{
    public class LogMessage
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string Timestamp { get; set; }
        public string CorrelationId { get; set; }
        public string Severity { get; set; }
        public string Payload { get; set; }
    }
}