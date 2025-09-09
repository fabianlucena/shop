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

        public Int64? AggregattedSizeItemsImagesCount { get; set; }

        public Int64? EnabledAggregattedSizeItemsImagesCount { get; set; }
    }
}