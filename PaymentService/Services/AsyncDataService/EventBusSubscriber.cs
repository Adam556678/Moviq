
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentService.Services.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentService.Services.AsyncDataService
{
    public class EventBusSubscriber : BackgroundService
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventBusSubscriber(
            IConfiguration configuration, 
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
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
                exchange: "payments.events",
                type: ExchangeType.Direct, 
                durable: true
            );

            await _channel.QueueDeclareAsync(
                queue: "reservation.payments",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await _channel.QueueBindAsync(
                queue: "reservation.payments",
                exchange: "payments.events",
                routingKey: "reservation.created"
            );

            Console.WriteLine("--> Listening for payment events");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMQ();

            var consumer = new AsyncEventingBasicConsumer(_channel!);

            // Listening for upcoming messages/events
            consumer.ReceivedAsync += async (sender, args) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentsService>();

                var routingKey = args.RoutingKey;
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                Console.WriteLine($"Event received: {routingKey}");

                if (routingKey == "reservation.created")
                {
                    var reservationCreatedEvent = JsonSerializer
                        .Deserialize<ReservationCreatedEvent>(body);

                    if (reservationCreatedEvent == null)
                        throw new InvalidOperationException("Event deserialization failed");

                    // Create payment session and save to DB
                    await paymentService.CreateCheckoutSessionAsync(reservationCreatedEvent);
                    await paymentService.SavePaymentAsync(reservationCreatedEvent);
                }

                // message processed, delete from queue
                await _channel!.BasicAckAsync(args.DeliveryTag, multiple: false);
            };

            await _channel!.BasicConsumeAsync(
                queue: "reservation.payments",
                autoAck: false,
                consumer: consumer
            );

        }
    }
}