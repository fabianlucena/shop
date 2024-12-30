using Microsoft.SqlServer.Types;
using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("Stores", Schema = "shop")]
    public class Store
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        [ForeignKey("Business")]
        public Int64 BusinessId { get; set; } = default;
        public Business? Business { get; set; } = default;

        public string? Description { get; set; }

        public SqlGeography? Location { get; set; }
    }
}