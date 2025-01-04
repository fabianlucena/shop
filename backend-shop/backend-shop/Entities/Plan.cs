using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("Plans", Schema = "shop")]
    public class Plan
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        public string Description { get; set; } = string.Empty;

        public int? MaxTotalCommerces { get; set; }

        public int? MaxEnabledCommerces { get; set; }

        public int? MaxTotalStores { get; set; }

        public int? MaxEnabledStores { get; set; }

        public int? MaxTotalItems { get; set; }

        public int? MaxEnabledItems { get; set; }
    }
}