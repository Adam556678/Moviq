var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

foreach (var service in new[] {"Users", "Movies", "Theater", "Reservation"})
{
    builder.Services.AddHttpClient(service, 
        c => c.BaseAddress = new Uri(builder.Configuration[$"ServiceUrls:{service}"]!));
}

builder.Services
    .AddGraphQLServer()
    .AddRemoteSchema("Users")
    .AddRemoteSchema("Movies")
    .AddRemoteSchema("Theater")
    .AddRemoteSchema("Reservation");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.MapGraphQL();

app.Run();