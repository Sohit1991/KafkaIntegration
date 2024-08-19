
namespace KafkaProducer.KafkaProdDepend
{
    public interface IWeatherDataPublisher
    {
        Task ProduceMessage(Weather weather);
    }
}