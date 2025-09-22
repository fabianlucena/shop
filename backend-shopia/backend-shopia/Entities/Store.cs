using NetTopologySuite.Geometries;
using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities
{
    [Table("Stores", Schema = "shopia")]
    public class Store
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        [ForeignKey("Commerce")]
        public Int64 CommerceId { get; set; } = default;
        public Commerce? Commerce { get; set; } = default;

        public string? Description { get; set; }

        public Point? Location { get; set; }
    }
}