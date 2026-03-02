using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Services;
using PaymentService.Services.AsyncDataService;
using Stripe;
using Stripe.Checkout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentConn")));
    
builder.Services.AddScoped<SessionService>();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddScoped<IPaymentsService, StripePaymentService>();
builder.Services.AddHostedService<EventBusSubscriber>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();