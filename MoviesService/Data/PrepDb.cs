using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesService.Models;

namespace MoviesService.Data
{
    static class PrepDb
    {
        public static async Task PrepPopulation(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                await SeedData(scope.ServiceProvider.GetService<AppDbContext>()!);                
            }
        }

        public static async Task SeedData(AppDbContext context)
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

            // 2. Seed Genres and Movies together to handle the relationship
            if (!context.Genres.Any() && !context.Movies.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                // Define Genres first
                var action = new Genre { Name = "Action", Movies = new List<Movie>()  };
                var scifi = new Genre { Name = "Sci-Fi", Movies = new List<Movie>()   };
                var drama = new Genre { Name = "Drama", Movies = new List<Movie>()   };

                // Define Movies and link to the Genre objects above
                var movies = new List<Movie>
                {
                    new Movie 
                    { 
                        Title = "Inception", 
                        Synopsis = "A thief who steals corporate secrets through the use of dream-sharing technology.", 
                        ReleaseDate = new DateTime(2010, 7, 16, 0, 0, 0, DateTimeKind.Utc),
                        Duration = 148,
                        Language = "English",
                        Genres = new List<Genre> { scifi, action } 
                    },
                    new Movie 
                    { 
                        Title = "The Dark Knight", 
                        Synopsis = "Batman raises the stakes in his war on crime.", 
                        ReleaseDate = new DateTime(2008, 7, 18, 0, 0, 0, DateTimeKind.Utc),
                        Duration = 152,
                        Language = "English",
                        Genres = new List<Genre> { action, drama } 
                    }
                };

                await context.Genres.AddRangeAsync(action, scifi, drama);
                await context.Movies.AddRangeAsync(movies);

                await context.SaveChangesAsync();
                Console.WriteLine("--> Seeding complete!");

            }
            else
            {
                Console.WriteLine("--> Data already exists, skipping seed.");
            }
        }
    
    }    
}