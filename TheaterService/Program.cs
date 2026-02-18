using Microsoft.EntityFrameworkCore;
using TheaterService.Data;
using TheaterService.Services;
using TheaterService.Services.AsyncDataService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("TheaterConn")));

builder.Services.AddScoped<IMoviesService, MoviesService>();
builder.Services.AddHostedService<EventBusSubscriber>();

builder.Services.AddScoped<IHallRepository, HallRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();