using System;
using Confluent.Kafka;


namespace GeneralProducer.Handlers
{
    public class Producer
    {
        private readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        public string _pubTopic = "";

        public Producer(string topic)
        {
            _pubTopic = topic;
        }

        public Producer() { }

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

    }
}
