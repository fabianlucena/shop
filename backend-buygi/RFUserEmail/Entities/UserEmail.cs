using RFService.EntitiesLib;

namespace RFUserEmail.Entities
{
    public class UserEmail : EntityTimestampsIdUuid
    {
        public Int64 UserId { get; set; }

        public required string Email { get; set; }
    }
}
