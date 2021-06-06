using System;

namespace Trigger
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string brokerAddress = "localhost:29092";
            string loggerUrl = "http://localhost:3000/api/logs";
            string topicName = "inbox";
            string inputFile = @"D:\Projects\Docker\exp30\mock_app\Trigger\payload.dat";

            if (args.Length >= 3)
            {
                brokerAddress = args[0];
                topicName = args[1];
                loggerUrl = args[2];
                if(args.Length > 3)
                    inputFile= args[3];
            }

            _ = new TriggerRunner(new TriggerRunnerConfiguration
            {
                BrokerAddress = brokerAddress,
                LoggerUrl = loggerUrl,
                TopicName = topicName,
                InputFile = inputFile
            });
        }
    }
}
