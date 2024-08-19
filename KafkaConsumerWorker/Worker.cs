using Confluent.Kafka;
using System.Text.Json;

namespace KafkaConsumerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                ClientId = "consumer-client",
                GroupId = "consumergroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build())
            {
                consumer.Subscribe("productTopic");
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumerData = consumer.Consume(TimeSpan.FromSeconds(3));
                    if (consumerData is not null)
                    {
                        var product = JsonSerializer.Deserialize<Product>(consumerData.Message.Value);
                        Console.WriteLine($"ProductId: {product?.Id} \n ProductName:{product?.Name}");

                        var weather = JsonSerializer.Deserialize<Weather>(consumerData.Message.Value);
                        Console.WriteLine($"Temperature: {weather?.temperature} \n State:{weather?.state}");
                    }
                }
            }
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            //await Task.Delay(1000, stoppingToken);
        }
    }
    public record Weather(string state, int temperature);
}
