using RFService.EntitiesLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    public class Password : EntityTimestampsIdUuid
    {
        [Unique]
        [ForeignKey("User")]
        public Int64 UserId { get; set; }

        [MaxLength(255)]
        public required string Hash { get; set; }
    }
}