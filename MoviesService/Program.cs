using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesService.Data;
using MoviesService.Services.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(opt =>
    {
        opt.IncludeExceptionDetails = true; // Exception Details (Remove later)
    })
    .AddAuthorization()
    .AddQueryType<MoviesService.GraphQL.Query>()
    .AddMutationType<MoviesService.GraphQL.Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

builder.Services.AddScoped<IMovieRepo, MovieRepo>();
builder.Services.AddScoped<IGenreRepo, GenreRepo>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();


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

await PrepDb.PrepPopulation(app);

app.Run();
