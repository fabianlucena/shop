using RFService.EntitiesLib;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Users", Schema = "auth")]
    public class User : EntityTimestampsIdUuidEnabled
    {
        [ForeignKey("Type")]
        public Int64 TypeId { get; set; }
        public UserType? Type { get; set; }

        [MaxLength(255)]
        public required string Username { get; set; }

        [MaxLength(255)]
        public required string FullName { get; set; }
    }
}