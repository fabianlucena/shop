using RFService.Exceptions;
using RFService.EntitiesLib;
using RFService.IRepo;

namespace RFService.ServicesLib
{
    public abstract class ServiceTimestampsId<Repo, Entity>(Repo repo) : ServiceTimestamps<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsId
    {
        public override async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Id != 0)
            {
                throw new ForbidenIdForCreationException();
            }

            return data;
        }
    }
}