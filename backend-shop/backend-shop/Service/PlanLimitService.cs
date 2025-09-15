using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using RFService.IRepo;
using RFService.Services;

namespace backend_shop.Service
{
    public class PlanLimitService(
        IRepo<PlanLimit> repo
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<PlanLimit>, PlanLimit>(repo),
            IPlanLimitService
    {
        public override async Task<PlanLimit> ValidateForCreationAsync(PlanLimit data)
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
