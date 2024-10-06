namespace backend_buygi.EntitiesLib
{
    public abstract class EntityTimestampsIdUuidEnabled : EntityTimestampsIdUuid
    {
        public bool IsEnabled { get; set; }
    }
}