namespace Trigger
{
    public class TriggerRunnerConfiguration
    {
        public string BrokerAddress { get; set; }
        public string TopicName { get; set; }
        public string LoggerUrl { get; set; }
        public string InputFile { get; set; }
        public string SenderId { get; set; }
    }
}