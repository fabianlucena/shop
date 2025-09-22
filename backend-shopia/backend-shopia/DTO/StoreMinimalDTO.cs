namespace backend_shopia.DTO
{
    public class StoreMinimalDTO
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }
    }
}
