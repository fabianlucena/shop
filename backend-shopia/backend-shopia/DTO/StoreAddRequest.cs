using backend_shopia.Types;

namespace backend_shopia.DTO
{
    public class StoreAddRequest
    {
        public required Guid CommerceUuid { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public LatLng? Location { get; set; }
    }
}
