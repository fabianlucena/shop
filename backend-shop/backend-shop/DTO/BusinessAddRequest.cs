namespace backend_shop.DTO
{
    public class BusinessAddRequest
    {
        public bool? IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }
    }
}
