namespace backend_shop.DTO
{
    public class CategoryMinimalDTO
    {
        public Guid Uuid { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }
    }
}
