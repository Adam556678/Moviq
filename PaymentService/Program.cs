using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Services;
using PaymentService.Services.AsyncDataService;
using PaymentService.Services.SyncDataService;
using Stripe;
using Stripe.Checkout;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // Port 5173: Dedicated to REST/Webhooks (HTTP/1.1)
    options.Listen(System.Net.IPAddress.Any ,5173, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1);
    
    // Port 5174: Dedicated to gRPC (HTTP/2)
    options.Listen(System.Net.IPAddress.Any, 5174, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddGrpc();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentConn")));
    
builder.Services.AddScoped<SessionService>();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddScoped<IPaymentsService, StripePaymentService>();
// builder.Services.AddHostedService<EventBusSubscriber>();
builder.Services.AddSingleton<EventBusPublisher>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Map the gRPC Service endpoint
app.MapGrpcService<GrpcPaymentService>();

app.MapGet("/grpc", () => "Communication with gRPC endpoints must be made through a gRPC client.");

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();