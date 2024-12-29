using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;

namespace backend_shop.Service
{
    public class PlanFeatureService(
        IRepo<PlanFeature> repo
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<PlanFeature>, PlanFeature>(repo),
            IPlanFeatureService
    {
        public override async Task<PlanFeature> ValidateForCreationAsync(PlanFeature data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.PlanId <= 0)
            {
                data.PlanId = data.Plan?.Id ?? 0;
                if (data.PlanId <= 0)
                    throw new NoPlanException();
            }

            var existent = await GetSingleOrDefaultForNameAsync(data.Name);
            if (existent != null)
                throw new PlanFeatureAlreadyExistsException();

            return data;
        }
    }
}
