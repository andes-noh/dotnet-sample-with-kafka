using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
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

        static async Task CreateTopicAsync(string bootstrapServers, string topicName)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = topicName, ReplicationFactor = 1, NumPartitions = 1 } });
                }
                catch (CreateTopicsException e)
                {
                    Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // create topic
            // if topic is exist, throw exception
            // if topic isn`t exist, make a topic
            CreateTopicAsync("localhost:9092", _props._subTopic);
            Task.Run(() => ConsumerRun(stoppingToken));
            return Task.CompletedTask;
        }
    }
}
