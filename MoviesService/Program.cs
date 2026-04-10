using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesService.Data;
using MoviesService.GraphQL;
using MoviesService.Services.AsyncDataService;
using MoviesService.Services.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MoviesConn")));

builder.Services.AddStackExchangeRedisCache(option =>
{
 option.Configuration = builder.Configuration.GetConnectionString("Redis");   
 option.InstanceName = "Movies_";
});

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
            ),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Look for JWT in cookie named "jwt"
                if (context.Request.Cookies.ContainsKey("jwt"))
                {
                    context.Token = context.Request.Cookies["jwt"];
                    Console.WriteLine("---> MOVIES SERVICE: JWT Cookie Received");
                }
                else
                {
                    Console.WriteLine("---> MOVIES SERVICE: JWT Cookie not received");
                }
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"--> MOVIES: Auth Failed! Reason: {context.Exception.Message}");
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("--> MOVIES: Token successfully validated!");
                return Task.CompletedTask;
            }
        };
    });

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(opt =>
    {
        opt.IncludeExceptionDetails = true; // Exception Details (Remove later)
    })
    .AddAuthorization()
    .AddQueryType<MoviesService.GraphQL.Query>()
    .AddTypeExtension<AuthQuery>() // REMOVE LATER
    .AddMutationType<MoviesService.GraphQL.Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting().
    AllowIntrospection(true);

builder.Services.AddScoped<IMovieRepo, MovieRepo>();
builder.Services.AddScoped<IGenreRepo, GenreRepo>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddSingleton<IEventBusClient, EventBusClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

// await PrepDb.PrepPopulation(app);
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
