namespace RFAuth.DTO
{
    public class AuthorizationResponse
    {
        public required string Username { get; set; }

        public required string DisplayName { get; set; }

        public required string AuthorizationToken { get; set; }
    }
}
