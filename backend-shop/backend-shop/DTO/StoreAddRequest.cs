using backend_shop.Types;

namespace backend_shop.DTO
{
    public class StoreAddRequest
    {
        public required Guid BusinessUuid { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public LatLng? Location { get; set; }
    }
}
