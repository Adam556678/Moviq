using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentService;
using ReservationService.Data;
using ReservationService.GraphQL;
using ReservationService.Services;
using ReservationService.Services.AsyncDataService;
using ReservationService.Services.SyncDataService.gRPC;

// Allow gRPC to use HTTP/2 without TLS
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReservationConn")));

// JWT autherization configuration
builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Look for JWT in cookie named "jwt"
                if (context.Request.Cookies.ContainsKey("jwt"))
                {
                    context.Token = context.Request.Cookies["jwt"];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddGrpc(); 
builder.Services.AddGrpcClient<PaymentGrpc.PaymentGrpcClient>(o =>
{
    o.Address = new Uri(builder.Configuration["GrpcPaymentService"]!);
});
builder.Services.AddScoped<IPaymentDataClient, PaymentDataClient>();

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(opt =>
    {
        opt.IncludeExceptionDetails = true;
    })
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<TimeSpanType>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AllowIntrospection(true);

builder.Services.AddSingleton<EventBusPublisher>();
builder.Services.AddHostedService<TheaterEventsSubscriber>();
builder.Services.AddHostedService<PaymentEventsSubscriber>();

builder.Services.AddScoped<IShowtimeService, ShowtimeService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IShowtimePricingService, ShowtimePricingService>();
builder.Services.AddScoped<IReservationRepo, ReservationRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>(); 
    try 
    {
        Console.WriteLine("--> Running Migrations...");
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Could not run migrations: {ex.Message}");
    }
}

app.Run();