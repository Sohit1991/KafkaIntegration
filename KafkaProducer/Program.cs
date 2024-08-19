using Confluent.Kafka;
using KafkaProducer.KafkaProdDepend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
//using var producer=new ProducerBuilder<Null,string>(config).Build();
builder.Services.AddSingleton<IProducer<Null, string>>(x => new ProducerBuilder<Null, string>(config).Build());

builder.Services.AddSingleton<IWeatherDataPublisher, WeatherDataPublisher>();

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
