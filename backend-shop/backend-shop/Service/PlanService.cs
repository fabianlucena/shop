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

            var limits = new List<PlanLimit>();
            var extendedPlan = plan;
            while (extendedPlan != null)
            {
                if (extendedPlan.IsEnabled)
                {
                    var extendToOptions = new QueryOptions(options);
                    extendToOptions.AddFilter("PlanId", extendedPlan.Id);
                    extendToOptions.AddFilter(Op.NotIn("Name", limits.Select(l => l.Name)));
                    var extendToLimits = await planLimitService.GetListAsync(extendToOptions);
                    if (extendToLimits.Any())
                        limits.AddRange(extendToLimits);
                }

                if (extendedPlan.ExtendToId == null)
                    break;

                extendedPlan = await GetSingleOrDefaultForIdAsync(
                    extendedPlan.ExtendToId.Value,
                    new QueryOptions { Switches = { { "IncludeDisabled", true } } }
                );
            }

            var basePlan = await GetSingleOrDefaultForNameAsync("Base");
            if (basePlan is not null)
            {
                var extendToOptions = new QueryOptions(options);
                extendToOptions.AddFilter("PlanId", basePlan.Id);
                extendToOptions.AddFilter(Op.NotIn("Name", limits.Select(l => l.Name)));
                var extendToLimits = await planLimitService.GetListAsync(extendToOptions);
                if (extendToLimits.Any())
                    limits.AddRange(extendToLimits);
            }

            var result = new PlanLimits(limits);
            return result;
        }
    }
}
