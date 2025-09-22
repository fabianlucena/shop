using backend_shopia.Entities;
using RFService.IServices;

namespace backend_shopia.IServices
{
    public interface ICategoryService
        : IService<Category>,
            IServiceId<Category>,
            IServiceUuid<Category>,
            IServiceIdUuid<Category>,
            IServiceSoftDeleteUuid<Category>,
            IServiceName<Category>,
            IServiceIdUuidName<Category>
    {
    }
}
