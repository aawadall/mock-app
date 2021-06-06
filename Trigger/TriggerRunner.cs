using Confluent.Kafka;
using Logger;
using PubSub;
using System;
using System.IO;
using System.Threading;

namespace Trigger
{
    /// <summary>
    /// Responsible for running trigger test case
    /// </summary>
    public class TriggerRunner
    {
        private TriggerRunnerConfiguration _configuration;
        private LoggerClient _logger;
        private Producer _producer;
        private Random random = new Random();
        public TriggerRunner(TriggerRunnerConfiguration configuration)
        {
            _configuration = configuration;

            _configuration.SenderId = _configuration.SenderId ?? Guid.NewGuid().ToString();

            _logger = new LoggerClient(_configuration.LoggerUrl, _configuration.SenderId);
            _logger.Log($"Started trigger process : {_configuration.SenderId}");
            _logger.Log($"Configuration -> Broker Address: {_configuration.BrokerAddress}");
            _logger.Log($"Configuration -> Topic Name    : {_configuration.TopicName}");

            _logger.Log($"Configuration -> Input File    : {_configuration.InputFile}");
            
            _logger.Log($"Configuration -> Logger URL    : {_configuration.LoggerUrl}");

            _producer = new Producer(_configuration.BrokerAddress, _configuration.TopicName);
            _logger.Log($"Producer created");

            if (string.IsNullOrWhiteSpace(_configuration.InputFile))
            {
                // user input
                ProcessUserInput();
            }
            else
            {
                // process file 
                ProcessFile(_configuration.InputFile);
            }
        }

        private void ProcessFile(string inputFile)
        {
            _logger.LogInformation($"Processing input file {_configuration.InputFile}");
            string message;
            var file = new StreamReader(inputFile);
            while ((message = file.ReadLine()) != null)
            {
                Thread.Sleep(random.Next(200));
                var correlationId = Guid.NewGuid().ToString();
                _logger.LogInformation($"sending {message}", correlationId);
                _producer.Publish(message + $";{correlationId}");
                Console.WriteLine($"PROC-> {correlationId} [{message}]");
                if(message.Contains("GOODBYE"))
                {
                    _logger.LogInformation("This might be last message", correlationId);
                    Console.WriteLine("last message sent");
                }
            }
            file.Close();
        }

        private void ProcessUserInput()
        {
            _logger.LogInformation($"Processing user input");
            var terminate = false;
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                terminate = true;
            };
            Console.WriteLine("User entered messages, to terminate, press Ctrl + C");
            int counter = 0;
            while (!terminate)
            {
                string message;
                Console.Write($"Message[{counter++}] > ");
                try
                {
                    message = Console.ReadLine();
                }
                catch (IOException)
                {
                    _logger.LogError("IO Exception");
                    break;
                }
                if(string.IsNullOrWhiteSpace(message))
                {
                    _logger.LogWarning("Empty message sent");
                    break;
                }

                try
                {
                    var correlationId = Guid.NewGuid().ToString();
                    _logger.LogInformation($"sending {message}", correlationId);
                    _producer.Publish(message+$";{correlationId}");
                }
                catch (ProduceException<string, string> e)
                {
                    _logger.LogError($"failed to deliver message; {e.Error}");
                    
                }
            }
            
        }
    }
}
