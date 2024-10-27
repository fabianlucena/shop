using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabledNameTitleTranslatable : EntityTimestampsIdUuidEnabledNameTitle
    {
        [Required]
        public bool? IsTranslatable { get; set; }
    }
}