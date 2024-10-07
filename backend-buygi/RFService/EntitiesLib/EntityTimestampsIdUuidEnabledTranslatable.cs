namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabledTranslatable : EntityTimestampsIdUuidEnabled
    {
        public bool IsTranslatable { get; set; }
        public Int64 TranslationContextId { get; set; }
    }
}