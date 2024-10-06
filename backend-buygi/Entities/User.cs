using backend_buygi.EntitiesLib;

namespace backend_buygi.Entities
{
    public class User : EntityTimestampsIdUuidEnabledTranslatable
    {
        public required string Username { get; set; }
        public required string FullName { get; set; }
    }
}