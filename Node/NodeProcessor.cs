using Confluent.Kafka;
using Logger;
using PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node
{
    public class NodeProcessor
    {
        private NodeConfiguration nodeConfiguration;
        private LoggerClient _logger;
        private Producer _producer;
        private IConsumer<Ignore, string> consumer;
        private CancellationToken cancellationToken;
        private Random random = new Random();
        public NodeProcessor(NodeConfiguration nodeConfiguration)
        {
            this.nodeConfiguration = nodeConfiguration;
            this.nodeConfiguration.SenderId = this.nodeConfiguration.SenderId ?? Guid.NewGuid().ToString();
            this.cancellationToken = new CancellationToken();
            _logger = new LoggerClient(nodeConfiguration.LoggerUrl, nodeConfiguration.SenderId);

            _logger.LogInformation($"Node {nodeConfiguration.SenderId} initializing");
            _producer = new Producer(nodeConfiguration.BrokerAddress, nodeConfiguration.OutTopicName);
            _logger.LogDebug("Kafka Producer created");

            var config = new ConsumerConfig
            {
                BootstrapServers = nodeConfiguration.BrokerAddress,
                GroupId = "logger-demo",
                EnableAutoCommit = false,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true,
                PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky
            };

            consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(nodeConfiguration.InTopicName);
            _logger.LogDebug("Kafka Consumer created");

            Process();
            consumer.Dispose();
        }

        private void Process()
        {
            bool killPill = false;
            while (!killPill)
            {
                try
                {
                    var result = consumer.Consume(cancellationToken);
                    if (result.IsPartitionEOF)
                    {
                        _logger.LogInformation($"Reached end of topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
                        continue;
                    }
                    
                    var messageVector = result.Message.Value.Split(';');
                    var correlationId = messageVector[1];
                    var message = messageVector[0];
                    killPill = message.Contains("GOODBYE");
                    _logger.LogInformation($"Received message {message}", correlationId);
                    DoSomething(messageVector);
                    
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Consume Exception: {e.Error.Reason}");
                    
                }
            }
            _logger.LogInformation($"{nodeConfiguration.SenderId} exiting");
            Console.WriteLine($"{nodeConfiguration.SenderId} exiting");
        }

        // some random event 
        private void DoSomething(string[] messageVector)
        {
            Console.WriteLine($"[Node:{nodeConfiguration.InTopicName}->{nodeConfiguration.OutTopicName}]-> {messageVector[1]} : [{messageVector[0]}]");
            var success = random.NextDouble() > 0.5;
            var appendedData = Guid.NewGuid().ToString();
            if(!success && !messageVector[0].Contains("GOODBYE"))
            {
                _logger.LogError($"some random error", messageVector[1]);
                return;
            }

            Thread.Sleep(random.Next(400));
            var toSend = messageVector[0] + appendedData +";" + messageVector[1];
            _logger.LogInformation($"sending out [{toSend}]", messageVector[1]);
            _producer.Publish(toSend);

        }
    }
}
