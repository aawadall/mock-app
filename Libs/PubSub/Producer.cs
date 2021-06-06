using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace PubSub
{
    public class Producer
    {
        public string _topic { get; }
        public IProducer<string, string> _producer { get; }

        public Producer(string broker, string topic)
        {
            var config = new ProducerConfig { BootstrapServers = broker };
            _topic = topic;
            _producer = new ProducerBuilder<string, string>(config).Build();
            
        }

        
        public void Publish(string message)
        {
            _producer.Produce(_topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
        }
    }
}
