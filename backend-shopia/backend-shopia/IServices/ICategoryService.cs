using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
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
