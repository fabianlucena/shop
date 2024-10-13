using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabledNameTitleTranslatable : EntityTimestampsIdUuidEnabledNameTitle
    {
        public bool IsTranslatable { get; set; } = false;
    }
}