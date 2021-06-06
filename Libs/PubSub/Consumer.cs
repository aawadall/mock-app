using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSub
{
    public class Consumer : IDisposable
    {
        
        public IConsumer<Ignore, string> consumer;

        public Consumer(string brokerAddress, string topicName)
        {
        
            var config = new ConsumerConfig 
            {
                BootstrapServers = brokerAddress,
                GroupId = "logger-demo",
                EnableAutoCommit = false,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true,
                PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky
            };

            consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topicName);
        }

        public void Dispose() => consumer.Dispose();
    }
}
