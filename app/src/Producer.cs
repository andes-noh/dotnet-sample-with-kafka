using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GeneralProducer.Handlers
{
    public class Producer : BackgroundService
    {
        public class Props
        {
            public string? _pubTopic { get; init; }
            public string? _pubMsg { get; init; }
        }

        private readonly Props _props;

        private readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        public string _pubTopic = "";

        public Producer(Props props)
        {
            _props = props;
        }

        public static Producer FromConfig(IConfiguration config)
        {
            var props = new Props
            {
                _pubServer = config["PUB_SERVER"],
                _pubTopic = config["TOPIC"]
            };
            return new Producer(props);
        }

        public Object? SendToKafka(string topic, string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    //producer send message to kafka broker
                    return producer.ProduceAsync(topic, new Message<Null, string>
                    {
                        Value = message
                    }).GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return null;
        }

        public void HelloWorld(CancellationToken cancellationToken, string topic, string server)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                SendToKafka(topic, ser)
                Console.WriteLine($"Sample Project: {text}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // func
            // loop
            Task.Run(() => HelloWorld(stoppingToken, _props._pubTopic, _props._pubServer));
            return Task.CompletedTask;
        }
    }
}
