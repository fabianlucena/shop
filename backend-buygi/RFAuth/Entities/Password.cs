using RFAuth.Util;
using RFService.EntitiesLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Passwords", Schema = "auth")]
    [Index(nameof(UserId), IsUnique = true)]
    public class Password : EntityTimestampsIdUuid
    {
        [ForeignKey("User")]
        public Int64 UserId { get; set; }

        public User? User { get; set; }

        [MaxLength(255)]
        public required string Hash { get; set; }
    }
}