using RFService.EntitiesLib;
using RFService.IRepo;
using RFService.RepoLib;

namespace RFService.ServicesLib
{
    public abstract class ServiceTimestampsIdUuidEnabledNameTitle<Repo, Entity>(Repo repo) : ServiceTimestampsIdUuidEnabledName<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabledNameTitle
    {
        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Title", out object? value))
            {
                if (value == null || string.IsNullOrEmpty((string)value))
                {
                    options = new GetOptions(options);
                    options.Filters.Remove("Title");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
