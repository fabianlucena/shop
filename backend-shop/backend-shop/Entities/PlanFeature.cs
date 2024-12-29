using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("PlanFeatures", Schema = "shop")]
    public class PlanFeature
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {

        [Required]
        [ForeignKey("Plan")]
        public long PlanId { get; set; } = default;
        public Plan? Plan { get; set; } = default;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int MaxLimit { get; set; }
    }
}