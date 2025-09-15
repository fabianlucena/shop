using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using RFOperators;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shop.Service
{
    public class PlanService(
        IRepo<Plan> repo,
        IServiceProvider serviceProvider
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

        public async Task<Plan> GetBaseAsync()
            => await GetSingleForNameAsync("Base");

        public async Task<Plan> GetSingleOrBaseAsync(QueryOptions options)
            => await GetSingleOrDefaultAsync(options)
                ?? await GetBaseAsync();

        public async Task<PlanLimits> GetLimitsForPlanAsync(Plan plan, QueryOptions? options = null)
        {
            var planLimitService = serviceProvider.GetRequiredService<IPlanLimitService>();

            var localOptions = new QueryOptions(options);
            localOptions.AddFilter("PlanId", plan.Id);
            var limits = (await planLimitService.GetListAsync(localOptions))
                .ToList();

            var extendedPlan = plan;
            while (extendedPlan.ExtendToId != null)
            {
                extendedPlan = await GetSingleOrDefaultForIdAsync(extendedPlan.ExtendToId.Value);
                if (extendedPlan == null)
                    break;

                var extendToOptions = new QueryOptions(options);
                extendToOptions.AddFilter("PlanId", extendedPlan.Id);
                extendToOptions.AddFilter(Op.NotIn("Name", limits.Select(l => l.Name)));
                var extendToLimits = await planLimitService.GetListAsync(extendToOptions);

                limits.AddRange(extendToLimits);
            }

            var result = new PlanLimits(limits);
            return result;
        }
    }
}
