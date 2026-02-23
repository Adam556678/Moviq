using Microsoft.EntityFrameworkCore;
using TheaterService.Data;
using TheaterService.GraphQL;
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
builder.Services.AddScoped<IShowtimeSeatService, ShowtimeSeatService>();
builder.Services.AddScoped<IShowtimeRepository, ShowtimeRepository>();

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(opt =>
    {
        opt.IncludeExceptionDetails = true;
    })
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<HallMutation>()
    .AddTypeExtension<ShowtimeMutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGraphQL();

await PrepDb.PrepPopulation(app);

app.Run();