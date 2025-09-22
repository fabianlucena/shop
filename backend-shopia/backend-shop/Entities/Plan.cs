using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("Plans", Schema = "shop")]
    public class Plan
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [ForeignKey("ExtendTo")]
        public Int64? ExtendToId { get; set; } = default;
        public Plan? ExtendTo { get; set; } = default;

        public string? Description { get; set; }
    }
}