using RFService.EntitiesLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;

namespace RFAuth.Entities
{
    public class Session : EntityTimestampsIdUuid
    {
        public Int64 UserId { get; set; }

        public Int64 DeviceId { get; set; }

        [Unique]
        [MaxLength(255)]
        public required string Token { get; set; }

        [Unique]
        [MaxLength(255)]
        public required string AutoLoginToken { get; set; }
    }
}