using backend_shopia.Types;

namespace backend_shopia.DTO
{
    public class StoreResponse
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public required LatLng Location { get; set; }
    }
}
