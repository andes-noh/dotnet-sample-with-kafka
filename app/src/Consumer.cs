using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using GeneralProducer.Handlers;

namespace GeneralConsumer.Handlers
{
    public class Consumer
    {
        public string _revTopic = "";

        public Consumer(string topic)
        {
            _revTopic = topic;
        }

        public Consumer() { }

        public void ConsumerRun(CancellationToken cancellationToken)
        {
            var _producer = new Producer();
            var conf = new ConsumerConfig
            {
                //
                GroupId = "test_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            using (var builder = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                builder.Subscribe(_revTopic);
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

        public Task startKafka(CancellationToken stoppingToken)
        {
            //
            Task.Run(() => ConsumerRun(stoppingToken));
            return Task.CompletedTask;
        }
    }
}
