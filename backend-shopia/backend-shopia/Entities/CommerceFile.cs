using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities
{
    [Table("CommercesFiles", Schema = "shopia")]
    public class CommerceFile
        : EntityCreatedAtIdUuidName
    {
        [Required]
        [ForeignKey("Commerce")]
        public Int64 CommerceId { get; set; } = default;
        public Commerce? Commerce { get; set; } = default;

        [Required]
        public string ContentType { get; set; } = "";

        [Required]
        public byte[] Content { get; set; } = [];
    }
}