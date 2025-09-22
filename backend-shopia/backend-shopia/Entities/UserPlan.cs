using RFAuth.Entities;
using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities
{
    [Table("Users_Plans", Schema = "shopia")]
    public class UserPlan
        : EntityTimestampsIdUuidEnabled
    {
        [Required]
        [ForeignKey("User")]
        public long UserId { get; set; } = default;
        public User? User { get; set; } = default;

        [Required]
        [ForeignKey("Plan")]
        public long PlanId { get; set; } = default;
        public Plan? Plan { get; set; } = default;

        public DateTime ExpirationDate { get; set; } = default;
    }
}