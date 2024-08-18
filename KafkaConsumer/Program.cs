// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using KafkaConsumer;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var consumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    ClientId = "consumer-client",
    GroupId = "cons-group"
};
using (var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build())
{
    consumer.Subscribe("productTopic");
    var consumerData = consumer.Consume(TimeSpan.FromSeconds(3));
    if (consumerData is not null)
    {
        var product = JsonSerializer.Deserialize<Product>(consumerData.Message.Value);
        Console.WriteLine($"ProductId: {product.Id} \n ProductName:{product.Name}");
    }
}
