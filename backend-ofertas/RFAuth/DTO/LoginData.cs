namespace RFAuth.DTO
{
    public class LoginData
    {
        public string? Username { get; set; }
        
        public string? Password { get; set; }

        public string? DeviceToken { get; set; }

        public string? AutoLoginToken { get; set; }
    }
}
