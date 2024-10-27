using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabled : EntityTimestampsIdUuid
    {
        [Required]
        public bool? IsEnabled { get; set; }
    }
}