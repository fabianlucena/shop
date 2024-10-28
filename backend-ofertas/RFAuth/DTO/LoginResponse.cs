using RFService.EntitiesLib;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RFAuth.DTO
{
    public class LoginResponse : EntityAttributes
    {
        public string AuthorizationToken { get; set; } = string.Empty;

        public string AutoLoginToken { get; set; } = string.Empty;

        public string DeviceToken { get; set; } = string.Empty;
    }
}
