using UsersService.Data;
using UsersService.DTOs;
using UsersService.Enums;
using UsersService.GraphQL.Types;
using UsersService.Helpers;
using UsersService.Models;

namespace UsersService.GraphQL
{
    public class Mutation
    {
        public async Task<LoginResult> Login(
            UserLoginDto input,
            [Service] IAuthRepository authRepo,
            [Service] IHttpContextAccessor httpContextAccessor
        )
        {
            var resp = await authRepo.LoginAsync(input);

            if (resp.LoginState == LoginState.InvalidCredentials)
            {
                return new LoginResult
                {
                    Message = "Invalid User Credentials"
                };
            }else if (resp.LoginState == LoginState.NotVerified)
            {
                return new LoginResult
                {
                    Message = "Email is Not Verified"
                };
                
            }

            var context = httpContextAccessor.HttpContext!;

            context.Response.Cookies.Append(
                "jwt",
                resp.Token!,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None, // Cookie is sent on ALL requests
                    Expires = DateTime.UtcNow.AddDays(1)
                }
            );

            // Valid Credentials resp
            return new LoginResult
            {
                Success = true,
                Message = "Logged in successfully",
                Token = resp.Token
            };
        }

        public async Task<RegisterResult> Register(
            UserRegisterDto input,
            [Service] IAuthRepository authRepository
        )
        {
            if (!PasswordValidator.IsStrongPassword(input.Password))
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = "Password is not strong enough"
                };
            }
            
            var resp = await authRepository.RegisterAsync(input);
            if (resp == null)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = "User with this email already exists"
                };
            }


            // Registered successfully
            return new RegisterResult
            {
                Success = true,
                Message = $"An OTP Verification code was sent to {input.Email}"
            };
        }

        public async Task<VerifyResult> Verify(
            VerifyUserDto input,
            [Service] IAuthRepository authRepository
        )
        {
            var res = await authRepository.Verify(input);

            return res switch
            {
                OTPResult.ExpiredOTP => new VerifyResult
                {
                    Message = "OTP expired, a new one was sent."
                },
                OTPResult.InvalidOTP => new VerifyResult
                {
                    Message = "Invalid OTP code."
                },

                OTPResult.OTPNotFound => new VerifyResult
                {
                    Message = "OTP not found. A new one was sent."
                },

                OTPResult.UserNotFound => new VerifyResult
                {
                    Message = "User not found."
                },

                _ => new VerifyResult
                {
                    Message = "Account verified successfully."
                },
            };

        }
    }
}