using RFService.EntitiesLib;
using RFService.RepoLib;

namespace RFService.IService
{
    public interface IServiceAttributes
    {
        Task AddAttributesAsync(EntityAttributes data);
    }
}
