using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Models
{
    public class UserCredentials
    {
        [Key]
        public Guid UserId { get; set; } // The PK is also the FK

        [Required]
        public required string HashedPassword { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = default!;
    }
}