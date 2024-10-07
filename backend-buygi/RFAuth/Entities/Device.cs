using RFService.EntitiesLib;

namespace RFAuth.Entities
{
    public class Device : EntityTimestampsIdUuid
    {
        public required string Token{ get; set; }
    }
}