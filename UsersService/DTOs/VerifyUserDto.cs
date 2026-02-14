namespace UsersService.DTOs
{
    public class VerifyUserDto
    {
        public required string Email { get; set; }
        public required string OTPCode { get; set; }
    }
}