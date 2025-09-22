namespace backend_shop.DTO
{
    public class ItemResponse
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required CategoryMinimalDTO Category { get; set; }

        public Guid? CategoryUuid { get; set; }

        public required StoreMinimalDTO Store { get; set; }

        public Guid? StoreUuid { get; set; }

        public required string Price { get; set; }

        public int? Stock { get; set; }

        public required bool IsPresent { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool IsMine { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ItemImageDTO[] Images { get; set; } = [];
    }
}
