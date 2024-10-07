using RFService.EntitiesLib;

namespace RFAuth.Entities
{
    public class Password : EntityTimestampsIdUuid
    {
        public Int64 UserId { get; set; }
        public required string Hash { get; set; }
    }
}