
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TheaterService.Services.Events;

namespace TheaterService.Services.AsyncDataService
{
    public class EventBusSubscriber : BackgroundService
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IConfiguration _configuration;

        public EventBusSubscriber(IConfiguration configuration)
        {
            _configuration = configuration;
            // TODO: Inject IServiceScopeFactory in the constructor
        }

        private async Task InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"] ?? "localhost", 
                Port = int.Parse(_configuration["RabbitMQPort"] ?? "5672")
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            
            await _channel.ExchangeDeclareAsync(exchange: "movies.events", type: ExchangeType.Direct);

            await _channel.QueueDeclareAsync(
                queue: "theater.movies",
                durable: true, //queue survives a broker restart.
                exclusive: false, //The queue is NOT restricted to one connection.
                autoDelete: false
            );

            await _channel.QueueBindAsync(
                queue: "theater.movies",
                exchange: "movies.events",
                routingKey: "movie.created"
            );

            await _channel.QueueBindAsync(
                queue: "theater.movies",
                exchange: "movies.events",
                routingKey: "movie.deleted"
            );

            Console.WriteLine("--> Listening for movie events...");

        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMQ();
            
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (sender, args)=> {
                var routingKey = args.RoutingKey;
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                Console.WriteLine($"Event received: {routingKey}");

                if (routingKey == "movie.created")
                {
                    var movieCreated = JsonSerializer
                        .Deserialize<MovieCreatedEvent>(body);

                    // TODO: add movie to DB
                    

                }
                else if (routingKey == "movie.deleted")
                {
                    var movieDeleted = 
                        JsonSerializer.Deserialize<MovieDeletedEvent>(body);

                    // TODO: Remove movie from db
                }

                // message processed, delete from queue
                await _channel!.BasicAckAsync(args.DeliveryTag, multiple: false);
            };

            await _channel!.BasicConsumeAsync(
                queue: "theater.movies",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}