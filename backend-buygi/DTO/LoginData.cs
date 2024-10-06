namespace backend_buygi.DTO
{
    public class LoginData
    {
        public required string Username { get; set; }
        
        public required string Password { get; set; }

        public string? DeviceToken { get; set; }
    }
}
