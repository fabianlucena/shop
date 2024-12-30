﻿using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;
using RFService.Repo;

namespace backend_shop.Service
{
    public class BusinessService(
        IRepo<Business> repo,
        IUserPlanService userPlanService
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Business>, Business>(repo),
            IBusinessService
    {
        public override async Task<Business> ValidateForCreationAsync(Business data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.OwnerId <= 0)
            {
                data.OwnerId = data.Owner?.Id ?? 0;
                if (data.OwnerId <= 0)
                    throw new NoOwnerException();
            }

            var existent = await GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = {
                    { "OwnerId", data.OwnerId},
                    { "Name", data.Name }
                }
            });

            if (existent != null)
                throw new ABusinessForThatNameAlreadyExistException();

            var enabledBusinessCount = await GetCountAsync(new GetOptions { Filters = { { "OwnerId", data.OwnerId } } });

            if (enabledBusinessCount >= (await userPlanService.GetMaxEnabledBusinessForCurrentUser()))
                throw new BusinessLimitReachedException();

            return data;
        }
    }
}
