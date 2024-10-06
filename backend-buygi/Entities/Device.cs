using backend_buygi.EntitiesLib;

namespace backend_buygi.Entities
{
    public class Device : EntityTimestampsIdUuid
    {
        public required string Token{ get; set; }
    }
}