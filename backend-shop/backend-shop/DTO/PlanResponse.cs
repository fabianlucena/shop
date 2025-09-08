namespace backend_shop.DTO
{
    public class PlanResponse
    {
        public Guid? Uuid { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? MaxTotalCommerces { get; set; }

        public int? MaxEnabledCommerces { get; set; }

        public int? MaxTotalStores { get; set; }

        public int? MaxEnabledStores { get; set; }

        public int? MaxTotalItems { get; set; }

        public int? MaxEnabledItems { get; set; }

        public int? MaxTotalItemsImages { get; set; }

        public int? MaxEnabledItemsImages { get; set; }

        public int? MaxAggregattedSizeItemsImages { get; set; }

        public int? MaxEnabledAggregattedSizeItemsImages { get; set; }
    }
}