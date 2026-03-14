
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TheaterService.Services.AsyncDataService
{
    public class SeatLockSubscriber : BackgroundService
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public SeatLockSubscriber(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory    
        )
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
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

            await _channel.ExchangeDeclareAsync(
                exchange: "reservation.events",
                type: ExchangeType.Direct, 
                durable: true
            );

            await _channel.QueueDeclareAsync(
                queue: "reservation.theater",
                durable: true, //queue survives a broker restart.
                exclusive: false, //The queue is NOT restricted to one connection.
                autoDelete: false
            );

            await _channel.QueueBindAsync(
                queue: "reservation.theater",
                exchange: "reservation.events",
                routingKey: "seat.locked"
            );

            Console.WriteLine("--> Listening for reservation events...");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMQ();

            var consumer = new AsyncEventingBasicConsumer(_channel!);

            // consumer.ReceivedAsync += (sender, args) =>
            // {
                
            // };
        }
    }
}