using backend_shop.Entities;
using RFRBAC.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IBusinessService
        : IService<Business>,
            IServiceId<Business>,
            IServiceUuid<Business>,
            IServiceSoftDeleteUuid<Business>,
            IServiceName<Business>,
            IServiceIdName<Business>
    {
    }
}
