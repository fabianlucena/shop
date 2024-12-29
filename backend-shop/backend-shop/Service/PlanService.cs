using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;
using RFService.Repo;

namespace backend_shop.Service
{
    public class PlanService(
        IRepo<Plan> repo
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Plan>, Plan>(repo),
            IPlanService
    {
        public override async Task<Plan> ValidateForCreationAsync(Plan data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            var existent = await GetSingleOrDefaultForNameAsync(data.Name);
            if (existent != null)
                throw new PlanAlreadyExistsException();

            return data;
        }
    }
}
