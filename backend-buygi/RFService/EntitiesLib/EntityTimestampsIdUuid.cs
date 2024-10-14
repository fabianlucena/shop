using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    [Index(nameof(Uuid), IsUnique = true)]
    public abstract class EntityTimestampsIdUuid : EntityTimestampsId
    {
        [Required]
        public Guid? Uuid { get; set; }
    }
}