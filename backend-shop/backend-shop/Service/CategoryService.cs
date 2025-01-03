using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;

namespace backend_shop.Service
{
    public class CategoryService(
        IRepo<Category> repo
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Category>, Category>(repo),
            ICategoryService
    {
    }
}

