using RFService.Services;
using RFService.IRepo;
using backend_shopia.Entities;
using backend_shopia.IServices;

namespace backend_shopia.Services
{
    public class CategoryService(
        IRepo<Category> repo
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Category>, Category>(repo),
            ICategoryService
    {
    }
}

