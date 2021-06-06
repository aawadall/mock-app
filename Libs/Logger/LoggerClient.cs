
using System.Text;
using System.Net;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace Logger
{
    public class LoggerClient
    {
        private readonly string PostMethodName = "POST";
        private readonly string JsonContentType = "application/json";
        private readonly IDictionary<string, string> Severity = new Dictionary<string, string>
        {
            { "information", "information" },
            { "warning", "warning" },
            { "error", "error" },
            { "debug", "debug" },
        };
        
        private readonly string LogServiceUrl;
        private readonly string DefaultSenderId;

        public LoggerClient(string logServiceUrl, string senderId = null)
        {
            LogServiceUrl = logServiceUrl;
            DefaultSenderId = senderId;
        }

        public void Log(LogMessage message)
        {
            // Set defaults
            message.Id = message.Id ?? Guid.NewGuid().ToString();
            message.Timestamp = message.Timestamp ?? DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
            message.Severity = message.Severity ?? Severity["information"];
            message.SenderId = message.SenderId ?? DefaultSenderId;

            var request = WebRequest.Create(LogServiceUrl);
            request.Method = PostMethodName;

            byte[] payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message,new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase}));
            request.ContentType = JsonContentType;
            request.ContentLength = payload.Length;

            using var requestStream = request.GetRequestStream();
            requestStream.Write(payload, 0, payload.Length);

            using var response = request.GetResponse();
            var statusCode = ((HttpWebResponse)response).StatusCode;

        }

        public void Log(string payload, string correlationId = null)
        {
            var message = new LogMessage
            {
                Payload = payload,
                CorrelationId = correlationId
            };
            Log(message);
        }

        public void LogInformation(string payload, string correlationId = null)
        {
            var message = new LogMessage
            {
                Payload = payload,
                Severity = Severity["information"],
                CorrelationId = correlationId
            };
            Log(message);
        }

        public void LogDebug(string payload, string correlationId = null)
        {
            var message = new LogMessage
            {
                Payload = payload,
                Severity = Severity["debug"],
                CorrelationId = correlationId
            };
            Log(message);
        }

        public void LogWarning(string payload, string correlationId = null)
        {
            var message = new LogMessage
            {
                Payload = payload,
                Severity = Severity["warning"],
                CorrelationId = correlationId
            };
            Log(message);
        }

        public void LogError(string payload, string correlationId = null)
        {
            var message = new LogMessage
            {
                Payload = payload,
                Severity = Severity["error"],
                CorrelationId = correlationId
            };
            Log(message);
        }
    }
}