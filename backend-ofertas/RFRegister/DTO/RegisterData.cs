using RFAuth.DTO;

namespace RFRegister.DTO
{
    public class RegisterData : UserAddRequest
    {
        public required string EMail { get; set; }

        public string? DeviceToken { get; set; }
    }
}
