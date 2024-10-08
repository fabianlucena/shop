using RFService.EntitiesLib;
using System.ComponentModel.DataAnnotations;

namespace RFAuth.Entities
{
    public class User : EntityTimestampsIdUuidEnabled
    {
        [MaxLength(255)]
        public required string Username { get; set; }

        [MaxLength(255)]
        public required string FullName { get; set; }
    }
}