using RFService.EntitiesLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    public class Session : EntityTimestampsIdUuid
    {
        [ForeignKey("User")]
        public Int64 UserId { get; set; }

        [ForeignKey("Device")]
        public Int64 DeviceId { get; set; }

        [Unique]
        [MaxLength(255)]
        public required string Token { get; set; }

        [Unique]
        [MaxLength(255)]
        public required string AutoLoginToken { get; set; }
    }
}