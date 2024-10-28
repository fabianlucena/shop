using RFService.EntitiesLib;
using RFService.RepoLib;
using RFService.IRepo;

namespace RFService.ServicesLib
{
    public abstract class ServiceAttributes
    {
        public virtual async Task AddAttributesAsync(EntityAttributes data)
        {
            data.Attributes ??= [];

            data.Attributes["IsValidEmail"] = false;
        }
    }
}