using System.ComponentModel.DataAnnotations;
using UsersService.Enums;
namespace UsersService.Models
{
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public bool IsVerified { get; set; }

        public UserCredentials UserCredentials { get; set; } = default!;

        public UserRole Role { get; set; } = UserRole.Customer;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
}