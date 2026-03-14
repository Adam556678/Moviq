
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReservationService.Models;
using ReservationService.Services.Events;

namespace ReservationService.Services.AsyncDataService
{
    public class EventBusSubscriber : BackgroundService
    {

        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventBusSubscriber(IConfiguration configuration, IServiceScopeFactory factory)
        {
            _configuration = configuration;
            _scopeFactory = factory;
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
                exchange: "theater.events",
                type: ExchangeType.Direct,
                durable: true
            );

            await _channel.QueueDeclareAsync(
                queue: "theater.reservation",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await _channel.QueueBindAsync(
                queue: "theater.reservation",
                exchange: "theater.events",
                routingKey: "showtime.created"
            );

            await _channel.QueueBindAsync(
                queue: "theater.reservation",
                exchange: "theater.events",
                routingKey: "showtime.deleted"
            );


            await _channel.QueueBindAsync(
                queue: "theater.reservation",
                exchange: "theater.events",
                routingKey: "showtime.pricing.created"
            );

            await _channel.QueueBindAsync(
                queue: "theater.reservation",
                exchange: "theater.events",
                routingKey: "seatLock.updated"
            );

            Console.WriteLine("--> Listening for theater events...");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMQ();

            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                using var scope = _scopeFactory.CreateScope();

                var showtimeService = scope.ServiceProvider.GetRequiredService<IShowtimeService>();
                var showtimePricingService = scope.ServiceProvider.GetRequiredService<IShowtimePricingService>();

                var routingKey = args.RoutingKey;
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                Console.WriteLine($"Event received: {routingKey}");

                if (routingKey == "showtime.created")
                {
                    var showtimeCreatedEvent = JsonSerializer
                        .Deserialize<ShowtimeCreatedEvent>(body);

                    if (showtimeCreatedEvent == null)
                        throw new Exception("showtimeCreated deserialization failed");

                    await showtimeService.CreateShowtimeAsync(new Showtime
                    {
                        MovieName = showtimeCreatedEvent.MovieTitle,
                        ShowtimeId = showtimeCreatedEvent.ShowtimeId,
                        StartTime = showtimeCreatedEvent.StartTime
                    });

                }else if (routingKey == "showtime.deleted")
                {
                    var showtimeDeletedEvent = JsonSerializer
                        .Deserialize<ShowtimeDeletedEvent>(body);

                    if (showtimeDeletedEvent == null)
                        throw new Exception("showtimeDeleted deserialization failed");

                    await showtimeService.DeleteShowtimeAsync(showtimeDeletedEvent.ShowtimeId);

                }else if (routingKey == "showtime.pricing.created")
                {
                    var showtimePricingCreated = JsonSerializer
                        .Deserialize<ShowtimePricingPublishedEvent>(body);

                    if (showtimePricingCreated == null)
                        throw new Exception("showtimePricingPublished deserialization failed");

                    await showtimePricingService.CreateShowtimePricing(new ShowtimePricing
                        {
                            ShowtimeId = showtimePricingCreated.ShowtimeId,
                            CreatedAt = DateTime.UtcNow
                        },
                        showtimePricingCreated.SeatPrices
                    );
                }else if (routingKey == "seatLock.updated")
                {
                    var seatStatusUpdateResponse = JsonSerializer
                        .Deserialize<SeatStatusUpdateResponse>(body);

                    if (seatStatusUpdateResponse == null)
                        throw new Exception("SeatStatusUpdateResponse deserialization failed");

                    // TODO: Complete reservation
                    Console.WriteLine($"SeatStatusUpdatedResponse received, reservationId: {seatStatusUpdateResponse.ReservationId}, success: {seatStatusUpdateResponse.LockSucceeded}");
                }

                // message processed, delete from queue
                await _channel!.BasicAckAsync(args.DeliveryTag, multiple: false);
            };

            await _channel!.BasicConsumeAsync(
                queue: "theater.reservation",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}