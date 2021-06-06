

using System.Threading;

namespace Node
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string brokerAddress = args.Length > 0 ? args[0] : "localhost:29092";
            string inTopicName = args.Length > 1 ? args[1] : "inbox";
            string outTopicName = args.Length > 2 ? args[2] : "outbox";
            string loggerUrl = args.Length > 3 ? args[3] : "http://localhost:3000/api/logs";
            _ = new NodeProcessor(new NodeConfiguration
            {
                BrokerAddress = brokerAddress,
                InTopicName = inTopicName,
                OutTopicName = outTopicName,
                LoggerUrl = loggerUrl
            });

            for(int i =0; i < 10;i++)
            {
            System.Console.WriteLine("EXIT");
            Thread.Sleep(300);

            }
            return;
        }
    }
}
