using Microsoft.EntityFrameworkCore;
using TheaterService.Models;

namespace TheaterService.Data
{
    static class PrepDb
    {
        public static async Task PrepPopulation(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                await SeedHalls(scope.ServiceProvider.GetService<AppDbContext>()!);
            }
        }

        public static async Task SeedHalls(AppDbContext context)
        {
            // 1. Migrate first to create the DB and Tables
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not run migrations: {ex.Message}");
                return; // Stop if migration fails
            }

            if (!context.Seats.Any() && !context.Halls.Any())
            {

                int rows = 5;
                int cols = 5;

                var hall = new Hall
                {
                    Name = "1A",
                    NumRows = rows,
                    NumColumns = cols,
                    Seats = new List<Seat>()  
                };

                for (int i = 1; i <= rows; i++)
                {
                    for (int j = 1; j <= cols; j++)
                    {
                        hall.Seats.Add(new Seat
                        {
                            Row = i,
                            Column = j
                        });
                    }
                }

                // add seats & halls to db
                context.Halls.Add(hall);
                await context.SaveChangesAsync();

                Console.WriteLine("--> Seeding complete!");
            } else
            {
                Console.WriteLine("--> Data already exists, skipping seed.");
            }
        }
    }
}