using backend_buygi.EntitiesLib;

namespace backend_buygi.Entities
{
    public class Session : EntityTimestampsIdUuid
    {
        public Int64 UserId { get; set; }
        public Int64 DeviceId { get; set; }
        public required string Token { get; set; }
        public required string AutoLoginToken { get; set; }
    }
}