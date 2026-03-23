
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReservationService.Services.AsyncDataService
{
    public class PaymentEventsSubscriber : BackgroundService
    {

        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentEventsSubscriber(IConfiguration configuration, IServiceScopeFactory factory)
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
                exchange: "payments.response.events",
                type: ExchangeType.Direct,
                durable: true
            );

            await _channel.QueueDeclareAsync(
                queue: "payment.reservation",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await _channel.QueueBindAsync(
                queue: "payment.reservation",
                exchange: "payments.response.events",
                routingKey: "payment.succeeded"
            );

            await _channel.QueueBindAsync(
                queue: "payment.reservation",
                exchange: "payments.response.events",
                routingKey: "payment.expired"
            );

            await _channel.QueueBindAsync(
                queue: "payment.reservation",
                exchange: "payments.response.events",
                routingKey: "payment.failed"
            );

            Console.WriteLine("--> Listening for payment events...");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMQ();

            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                var routingKey = args.RoutingKey;
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                Console.WriteLine($"Event received: {routingKey}");

                await _channel!.BasicAckAsync(args.DeliveryTag, multiple: false);
            };

            await _channel!.BasicConsumeAsync(
                queue: "payment.reservation",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}