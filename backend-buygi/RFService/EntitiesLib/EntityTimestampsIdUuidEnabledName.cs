using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabledName : EntityTimestampsIdUuidEnabled
    {
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}