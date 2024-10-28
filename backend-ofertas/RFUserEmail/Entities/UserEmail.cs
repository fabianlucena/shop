using RFAuth.Entities;
using RFService.EntitiesLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RFUserEmail.Entities
{
    [Table("UsersEmails", Schema = "auth")]
    [Index(nameof(UserId), IsUnique = true)]
    public class UserEmail : EntityTimestampsIdUuid
    {
        [Required]
        [ForeignKey("User")]
        public Int64 UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required bool? IsVerified { get; set; }
    }
}
