using backend_buygi.EntitiesLib;

namespace backend_buygi.Entities
{
    public class Password : EntityTimestampsIdUuid
    {
        public Int64 UserId { get; set; }
        public required string Hash { get; set; }
    }
}