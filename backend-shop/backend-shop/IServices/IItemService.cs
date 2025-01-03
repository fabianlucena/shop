﻿using backend_shop.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
{
    public interface IItemService
        : IService<Item>,
            IServiceId<Item>,
            IServiceUuid<Item>,
            IServiceSoftDeleteUuid<Item>,
            IServiceName<Item>,
            IServiceIdUuidName<Item>
    {
        Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, GetOptions? options = null);

        Task<GetOptions> GetFilterForCurrentUserAsync(GetOptions? options = null);

        Task<Int64> GetCountForCurrentUserAsync(GetOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(GetOptions? options = null);
    }
}
