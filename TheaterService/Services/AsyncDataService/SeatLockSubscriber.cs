
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TheaterService.Services.Events;

namespace TheaterService.Services.AsyncDataService
{
    public class SeatLockSubscriber : BackgroundService
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IEventBusPublisher _eventBusPublisher;

        public SeatLockSubscriber(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory,
            IEventBusPublisher eventBusPublisher
        )
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _eventBusPublisher = eventBusPublisher;
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

            consumer.ReceivedAsync += async (sender, args) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var showtimeSeatService = scope.ServiceProvider.GetRequiredService<IShowtimeSeatService>();

                var routingKey = args.RoutingKey;
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                Console.WriteLine($"Event received: {routingKey}");

                if (routingKey == "seat.locked")
                {
                    var seatStatusUpdateRequest = JsonSerializer
                        .Deserialize<SeatStatusUpdateRequested>(body);
                    
                    if (seatStatusUpdateRequest is null)
                        throw new InvalidOperationException("SeatStatusUpdateRequest deserialization failed");

                    List<Guid> seatIds = [.. seatStatusUpdateRequest.SeatIds];

                    bool seatLockSuccess =  await showtimeSeatService.
                        TryLockSeatAsync(seatStatusUpdateRequest.ShowtimeId, seatIds);
                    
                    // Publish LockSeatStatusUpdated
                    var seatStatusUpdateResponse = new SeatStatusUpdateResponse
                    {
                        LockSucceeded = seatLockSuccess,
                        ReservationId = seatStatusUpdateRequest.ReservationId,
                        ShowtimeId = seatStatusUpdateRequest.ShowtimeId,
                        SeatIds = seatStatusUpdateRequest.SeatIds
                    };
                    await _eventBusPublisher.PublishSeatStatusUpdateResponse(seatStatusUpdateResponse);
                }

                // message processed, delete from queue
                await _channel!.BasicAckAsync(args.DeliveryTag, multiple: false);
            };

            await _channel!.BasicConsumeAsync(
                queue: "reservation.theater",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}