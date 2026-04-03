using Microsoft.EntityFrameworkCore;
using UsersService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("UsersConn")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .WithOrigins("http://localhost:4000")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<UsersService.GraphQL.Query>()
    .AddMutationType<UsersService.GraphQL.Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting().
    AllowIntrospection(true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

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

app.UseCors();

app.Run();