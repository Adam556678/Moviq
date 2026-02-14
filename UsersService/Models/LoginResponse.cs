namespace UsersService.Models
{
    public class LoginResponse
    {
        public LoginState LoginState { get; set; }
        public string? Token { get; set; }
    }

    public enum LoginState
    {
        InvalidCredentials,
        NotVerified,
        Success
    }
}