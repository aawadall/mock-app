using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    public class NodeConfiguration
    {
        public string BrokerAddress { get; internal set; }
        public string InTopicName { get; internal set; }
        public string OutTopicName { get; internal set; }
        public string LoggerUrl { get; internal set; }
        public string SenderId { get; set; }
    }
}
