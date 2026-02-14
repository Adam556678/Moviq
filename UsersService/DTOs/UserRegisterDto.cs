using UsersService.Enums;

namespace UsersService.DTOs
{
    public record UserRegisterDto(
        string Name,
        string Email,
        string Password
    );
}