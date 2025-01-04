namespace backend_shop.DTO
{
    public class CommerceAddRequest
    {
        public bool? IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }
    }
}
