using RFService.EntitiesLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;

namespace RFAuth.Entities
{
    public class Device : EntityTimestampsIdUuid
    {
        [Unique]
        [MaxLength(255)]
        public required string Token{ get; set; }
    }
}