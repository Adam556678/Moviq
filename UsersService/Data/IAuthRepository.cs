using UsersService.DTOs;
using UsersService.Enums;
using UsersService.Models;

namespace UsersService.Data
{
    public interface IAuthRepository
    {
        Task<User?> RegisterAsync(UserRegisterDto userRegister);

        Task<LoginResponse> LoginAsync(UserLoginDto userLogin);

        Task<OTPResult> Verify(VerifyUserDto verifyUser);
    }
}