using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using UsersService.DTOs;
using UsersService.Enums;
using UsersService.Models;

namespace UsersService.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(UserLoginDto userLogin)
        {
            // search for user in db
            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Email == userLogin.Email);
            if (user == null)
            {
                return new LoginResponse
                {
                    LoginState = LoginState.InvalidCredentials
                };
            }

            // get user's credentials
            var credentials = await _context.UserCredentials
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            // verify user's password
            var passowrdVerify = new PasswordHasher<User>().VerifyHashedPassword(
                user, credentials!.HashedPassword, userLogin.Password);
            if (passowrdVerify == PasswordVerificationResult.Failed)
            {
                return new LoginResponse
                {
                    LoginState = LoginState.InvalidCredentials
                };
            }

            // Check if user is verified
            if (!user.IsVerified)
            {
                return new LoginResponse
                {
                    LoginState = LoginState.NotVerified
                };
            }

            return new LoginResponse
            {
                LoginState = LoginState.Success,
                Token = CreateToken(user)
            };
        }

        public async Task<User?> RegisterAsync(UserRegisterDto userRegister)
        {
            // check for a user with the same Email
            var userExist = await _context.Users.FirstOrDefaultAsync(u => 
                u.Email == userRegister.Email);
            if (userExist != null)
            {
                return null;
            }

            // create user Entity
            var user = new User
            {
                Name = userRegister.Name,
                Email = userRegister.Email
            };

            // Hash password and store it
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, userRegister.Password);
            var userCredentials = new UserCredentials
            {
                User = user,
                HashedPassword = hashedPassword
            }; 

            // add entities to DB
            _context.Users.Add(user);
            _context.UserCredentials.Add(userCredentials);
            await _context.SaveChangesAsync();

            // Create OTP and send Email to user
            var otp = await CreateOTP(user.Id);
            await SendVerificationEmail(user.Email, otp);

            return user;            
        }

        public async Task<OTPResult> Verify(VerifyUserDto verifyUser)
        {
            // get user entity
            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Email == verifyUser.Email);
            
            if (user == null)
            {
                return OTPResult.UserNotFound;
            }

            // get otp code
            var otp = await _context.UserOTPs
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otp == null)
            {
                // resend OTP verification code
                var newOtp = await CreateOTP(user.Id);
                await SendVerificationEmail(user.Email, newOtp);

                return OTPResult.OTPNotFound;
            }
            
            // check OTP expiration
            if (otp.ExpiresAt < DateTime.UtcNow)
            {
                // Delete the expired OTP
                _context.UserOTPs.Remove(otp);
                await _context.SaveChangesAsync();

                // resend OTP verification code
                var newOtp = await CreateOTP(user.Id);
                await SendVerificationEmail(user.Email, newOtp);

                return OTPResult.ExpiredOTP;
            }

            // verify otp code
            if (otp.Otp != verifyUser.OTPCode)
            {
                return OTPResult.InvalidOTP;
            }


            // -- Valid otp code --
            
            // Delete otp from UserOTPs
            _context.UserOTPs.Remove(otp);

            // Mark user as verified
            user.IsVerified = true;
            
            await _context.SaveChangesAsync();

            return OTPResult.Success;
        }

        private async Task<string> CreateOTP(Guid userId)
        {
            var random = new Random();
            string otp = random.Next(1000, 9999).ToString();

            var userOTP = new UserOTP
            {
                Otp = otp,
                UserId = userId
            };

            _context.UserOTPs.Add(userOTP);
            await _context.SaveChangesAsync();

            return otp;
        }

        private async Task SendVerificationEmail(string toEmail, string otp)
        {
            var sender = _configuration["EmailSettings:FromEmail"];
            var appPassword = _configuration["EmailSettings:AppPassword"];
            var host = _configuration["EmailSettings:SMTPHost"];
            var port = _configuration.GetValue<int>("EmailSettings:SMTPPort");

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Moviq", sender));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = "Your Verification Code";

            email.Body = new TextPart("html")
            {
                Text = $"<h2>Your verification code is:</h2><h1>{otp}</h1><p>Expires in 1 hour.</p>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(sender, appPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        private string CreateToken(User user)
        {

            // Define user Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // Signing key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"), 
                audience: _configuration.GetValue<string>("AppSettings:Audience"), 
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}