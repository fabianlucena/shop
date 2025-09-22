using backend_shopia.Entities;

namespace backend_shopia.DTO
{
    public class CommerceResponse
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public required PlanDTO Plan { get; set; }
    }
}
