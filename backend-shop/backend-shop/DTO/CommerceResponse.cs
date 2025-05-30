﻿using backend_shop.Entities;

namespace backend_shop.DTO
{
    public class CommerceResponse
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public required PlanMinimalDTO Plan { get; set; }
    }
}
