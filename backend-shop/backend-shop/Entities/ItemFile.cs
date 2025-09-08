using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shop.Entities
{
    [Table("ItemsFiles", Schema = "shop")]
    public class ItemFile
        : EntityCreatedAtIdUuidName
    {
        [Required]
        [ForeignKey("Item")]
        public Int64 ItemId { get; set; } = default;
        public Item? Item { get; set; } = default;

        [Required]
        public string ContentType { get; set; } = "";

        [Required]
        public byte[] Content { get; set; } = [];
    }
}