using RFAuth.Entities;
using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("Companies", Schema = "shop")]
    public class Company
        : EntitySoftDeleteTimestampsIdUuidEnabled
    {
        [Required]
        [ForeignKey("Owner")]
        public long OwnerId { get; set; } = default;
        public User? Owner { get; set; } = default;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}