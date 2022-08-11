using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GeneralConsumer.Handlers
{
    public class Consumer : BackgroundService
    {
        public class Props
        {
            public string? _subTopic { get; init; }
        }

        private readonly Props _props;
        public string _revTopic = "";

        public Consumer(Props props)
        {
            _props = props;
        }

        public static Consumer FromConfig(IConfiguration config)
        {
            var props = new Props
            {
                _subTopic = config["TOPIC"],
            };
            return new Consumer(props);
        }

        public void ConsumerRun(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                //
                GroupId = "test_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            using (var builder = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                builder.Subscribe(_props._subTopic);
                var cancelToken = new CancellationTokenSource();
                try
                {

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumer = builder.Consume(cancelToken.Token);

                        // print message to console
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => ConsumerRun(stoppingToken));
            return Task.CompletedTask;
        }
    }
}
