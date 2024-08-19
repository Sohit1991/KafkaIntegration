using Confluent.Kafka;
using System.Text.Json;

namespace KafkaProducer.KafkaProdDepend
{
    public class WeatherDataPublisher : IWeatherDataPublisher
    {
        private readonly IProducer<Null, string> producer;

        public WeatherDataPublisher(IProducer<Null, string> producer)
        {
            this.producer = producer;
        }
        public async Task ProduceMessage(Weather weather) =>
            producer.Produce("productTopic", new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(weather)
            });
    }

    public record Weather(string state, int temperature);
}
