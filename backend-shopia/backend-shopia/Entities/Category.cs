using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities
{
    [Table("Categories", Schema = "shopia")]
    public class Category
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        public required string Description { get; set; }

    }
}