namespace backend_shop.DTO
{
    public class PlanDTO
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

        public int? MaxTotalImagesPerSingleItem { get; set; }

        public int? MaxEnabledItemsImages { get; set; }

        public Int64? MaxItemImageSize { get; set; }

        public Int64? MaxItemsImagesAggregatedSize { get; set; }

        public Int64? MaxEnabledItemsImagesAggregatedSize { get; set; }

        public int? MaxTotalCommercesImages { get; set; }

        public int? MaxTotalImagesPerSingleCommerce { get; set; }

        public int? MaxEnabledCommercesImages { get; set; }

        public Int64? MaxCommerceImageSize { get; set; }

        public Int64? MaxCommercesImagesAggregatedSize { get; set; }

        public Int64? MaxEnabledCommercesImagesAggregatedSize { get; set; }
    }
}