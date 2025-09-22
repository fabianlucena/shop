namespace backend_shop.DTO
{
    public class ItemAddRequest
    {
        public bool IsEnabled { get; set; } = true;

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required Guid CategoryUuid { get; set; }

        public required Guid StoreUuid { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsPresent { get; set; } = false;

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }
    }
}
