using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using TheaterService.Services.Events;

namespace TheaterService.Services.AsyncDataService
{
    public class EventBusPublisher : IEventBusPublisher
    {

        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;
        private IChannel? _channel;
        public EventBusPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQPort"] ?? "5672")
            };
        }

        public async Task PublishShowtimePricingCreated(ShowtimePricingPublishedEvent @event)
        {
            var message = JsonSerializer.Serialize(@event);

            await SendMessage(
                exchange: "theater.events",
                routingKey: "showtime.pricing.created",
                message
            );
        }


        public async Task PublishShowtimeCreated(ShowtimeCreatedEvent @event)
        {
            var message = JsonSerializer.Serialize(@event);

            await SendMessage(
                exchange: "theater.events",
                routingKey: "showtime.created",
                message
            );
        }

        public async Task PublishShowtimeDeleted(ShowtimeDeletedEvent @event)
        {
            var message = JsonSerializer.Serialize(@event);

            await SendMessage(
                exchange: "theater.events",
                routingKey: "showtime.deleted",
                message
            );
        }

        public async Task PublishSeatStatusUpdateResponse(SeatStatusUpdateResponse @event)
        {
            var message = JsonSerializer.Serialize(@event);

            await SendMessage(
                exchange: "theater.events",
                routingKey: "seatLock.updated",
                message
            );
        }

         private async Task EnsureConnection(string exchange)
        {
            if (_channel != null && _channel.IsOpen)
                return;

            Console.WriteLine("--> Attempting to connect to RabbitMQ...");

            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(
                exchange, 
                ExchangeType.Direct, 
                durable: true
            );
        }

        private async Task SendMessage(string exchange, string routingKey, string message)
        {
            await EnsureConnection(exchange);

            var body = Encoding.UTF8.GetBytes(message);
            await _channel!.BasicPublishAsync(exchange, routingKey, body);

            Console.WriteLine($"Message sent: {message}");
        }

        public async Task Dispose()
        {
            Console.WriteLine("Message Bus Disposed");
            if (_channel != null && _connection != null && _channel.IsOpen)
            {
                await _channel.CloseAsync();
                await _connection.CloseAsync();
            }
        }

    }
}