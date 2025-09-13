namespace backend_shop.DTO
{
    public class UsedPlanDTO
    {
        public int? TotalCommercesCount { get; set; }

        public int? EnabledCommercesCount { get; set; }

        public int? TotalStoresCount { get; set; }

        public int? EnabledStoresCount { get; set; }

        public int? TotalItemsCount { get; set; }

        public int? EnabledItemsCount { get; set; }

        public int? TotalItemsImagesCount { get; set; }

        public int? EnabledItemsImagesCount { get; set; }

        public Int64? ItemsImagesAggregatedSize { get; set; }

        public Int64? EnabledItemsImagesAggregatedSize { get; set; }

        public int? TotalCommercesImagesCount { get; set; }

        public int? EnabledCommercesImagesCount { get; set; }

        public Int64? CommercesImagesAggregatedSize { get; set; }

        public Int64? EnabledCommercesImagesAggregatedSize { get; set; }
    }
}