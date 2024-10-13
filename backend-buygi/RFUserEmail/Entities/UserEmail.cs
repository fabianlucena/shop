using RFAuth.Entities;
using RFService.EntitiesLib;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFUserEmail.Entities
{
    [Table("UsersEmails", Schema = "auth")]
    public class UserEmail : EntityTimestampsIdUuid
    {
        [ForeignKey("User")]
        public Int64 UserId { get; set; }
        public User? User { get; set; }

        public required string Email { get; set; }
    }
}
