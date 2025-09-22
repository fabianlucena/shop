using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using RFOperators;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shopia.Services
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

            var extendedPlans = new List<Int64>();
            var limits = new List<PlanLimit>();
            var extendedPlan = plan;
            while (extendedPlan != null)
            {
                if (extendedPlan.IsEnabled)
                {
                    var extendLimitsOptions = new QueryOptions(options);
                    extendLimitsOptions.AddFilter("PlanId", extendedPlan.Id);
                    var extendLimits = await planLimitService.GetListAsync(extendLimitsOptions);
                    foreach (var limit in extendLimits)
                    {
                        var current = limits.Find(l => l.Name == limit.Name);
                        if (current == null)
                            limits.Add(limit);
                        else if (limit.Limit > current.Limit)
                            current.Limit = limit.Limit;
                    }
                }

                if (extendedPlan.ExtendToId == null)
                    break;

                extendedPlans.Add(extendedPlan.Id);

                var extendToOptions = new QueryOptions
                {
                    Switches = { { "IncludeDisabled", true } },
                    Filters = { { "Id", extendedPlan.ExtendToId.Value } },
                };
                extendedPlan = await GetSingleOrDefaultAsync(extendToOptions);
                if (extendedPlan != null && extendedPlans.Contains(extendedPlan.Id))
                    break;
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
