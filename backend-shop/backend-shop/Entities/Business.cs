using RFAuth.Entities;
using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("Businesses", Schema = "shop")]
    public class Business
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        [ForeignKey("Owner")]
        public long OwnerId { get; set; } = default;
        public User? Owner { get; set; } = default;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Plan")]
        public long PlanId { get; set; } = default;
        public Plan? Plan { get; set; } = default;

    }
}