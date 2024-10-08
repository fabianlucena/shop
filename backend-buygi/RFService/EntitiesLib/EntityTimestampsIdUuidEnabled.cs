namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabled : EntityTimestampsIdUuid
    {
        public bool IsEnabled { get; set; } = true;
    }
}