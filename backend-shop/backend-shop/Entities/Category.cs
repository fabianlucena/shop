using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("Categories", Schema = "shop")]
    public class Category
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        public required string Description { get; set; }

    }
}