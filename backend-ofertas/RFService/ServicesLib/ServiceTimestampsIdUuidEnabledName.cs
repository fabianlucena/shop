using RFService.EntitiesLib;
using RFService.IRepo;
using RFService.RepoLib;

namespace RFService.ServicesLib
{
    public abstract class ServiceTimestampsIdUuidEnabledName<Repo, Entity>(Repo repo) : ServiceTimestampsIdUuidEnabled<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabledName
    {
        public async Task<Entity> GetSingleForNameAsync(string name)
        {
            return await repo.GetSingleAsync(new GetOptions
            {
                Filters = { { "Name", name } }
            });
        }

        public async Task<Entity?> GetSingleOrDefaultForNameAsync(string name)
        {
            return await repo.GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = { { "Name", name } }
            });
        }

        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Name", out object? value))
            {
                options = new GetOptions(options);
                if (value != null
                    && !string.IsNullOrEmpty((string)value)
                )
                {
                    options.Filters = new Dictionary<string, object?> { { "Name", value } };
                    return options;
                }
                else
                {
                    options.Filters.Remove("Name");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
