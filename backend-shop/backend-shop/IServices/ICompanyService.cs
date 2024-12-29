using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface ICompanyService
        : IService<Company>,
            IServiceId<Company>,
            IServiceUuid<Company>,
            IServiceSoftDeleteUuid<Company>
    {
    }
}
