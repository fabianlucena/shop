using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    public abstract class EntityTimestamps : Entity
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}