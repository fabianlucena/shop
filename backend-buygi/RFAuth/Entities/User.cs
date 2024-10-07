using RFService.EntitiesLib;

namespace RFAuth.Entities
{
    public class User : EntityTimestampsIdUuidEnabledTranslatable
    {
        public required string Username { get; set; }
        public required string FullName { get; set; }
    }
}