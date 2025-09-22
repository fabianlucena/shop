using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities
{
    [Table("PlansLimits", Schema = "shopia")]
    public class PlanLimit
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        [ForeignKey("Plan")]
        public Int64 PlanId { get; set; } = default;
        public Plan? Plan { get; set; } = default;

        public string? Description { get; set; }

        public Int64 Limit { get; set; }
    }
}