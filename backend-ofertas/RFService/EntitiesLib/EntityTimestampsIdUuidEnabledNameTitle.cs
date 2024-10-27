using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabledNameTitle : EntityTimestampsIdUuidEnabledName
    {
        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }
    }
}