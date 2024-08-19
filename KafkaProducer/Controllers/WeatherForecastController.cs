using Confluent.Kafka;
using KafkaProducer.KafkaProdDepend;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KafkaProducer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherDataPublisher weatherDataPublisher;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IWeatherDataPublisher weatherDataPublisher)
        {
            _logger = logger;
            this.weatherDataPublisher = weatherDataPublisher;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            Product product = new Product
            {
                Id = 1,
                Name = "Phone"
            };
            var message = new Message<Null, string>()
            {
                Value = JsonSerializer.Serialize(product)
            };

            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All
            };
            var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            producer.Produce("productTopic", message);
            producer.Dispose();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("getweatherdata")]
        public async Task GetWeatherData(Weather weather)
        {
            await weatherDataPublisher.ProduceMessage(weather);
            
        }
    }
}
