using NetTopologySuite.Geometries;
using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("Items", Schema = "shop")]
    public class Item
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        public required bool InheritedIsEnabled { get; set; }

        public Point? Location { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [ForeignKey("Category")]
        public Int64 CategoryId { get; set; } = default;
        public Category? Category { get; set; } = default;

        [Required]
        [ForeignKey("Store")]
        public Int64 StoreId { get; set; } = default;
        public Store? Store { get; set; } = default;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int? Stock { get; set; }

        [Required]
        public bool IsPresent { get; set; } = false;

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }
    }
}