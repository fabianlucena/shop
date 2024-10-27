namespace RFAuth.DTO
{
    public class AuthorizationResponse
    {
        public required string Username { get; set; }

        public required string FullName { get; set; }

        public required string AuthorizationToken { get; set; }
    }
}
