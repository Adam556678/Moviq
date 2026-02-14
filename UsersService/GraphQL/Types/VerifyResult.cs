namespace UsersService.GraphQL.Types
{
    public class VerifyResult
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
    }
}