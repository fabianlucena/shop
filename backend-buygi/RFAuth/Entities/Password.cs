using RFService.EntitiesLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;

namespace RFAuth.Entities
{
    public class Password : EntityTimestampsIdUuid
    {
        [Unique]
        public Int64 UserId { get; set; }

        [MaxLength(255)]
        public required string Hash { get; set; }
    }
}