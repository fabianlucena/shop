﻿namespace backend_shop.DTO
{
    public class StoreAddRequest
    {
        public required Guid BusinessUuid { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }
    }
}