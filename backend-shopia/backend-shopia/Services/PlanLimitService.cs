using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using RFService.IRepo;
using RFService.Services;

namespace backend_shopia.Services
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
