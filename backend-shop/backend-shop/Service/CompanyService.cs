using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;
using RFService.Repo;

namespace backend_shop.Service
{
    public class CompanyService(
        IRepo<Company> repo
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabled<IRepo<Company>, Company>(repo),
            ICompanyService
    {
        public override async Task<Company> ValidateForCreationAsync(Company data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Name == null)

            if (data.OwnerId <= 0)
            {
                data.OwnerId = data.Owner?.Id ?? 0;
                if (data.OwnerId <= 0)
                {
                    throw new NoOwnerException();
                }
            }

            var existent = await GetSingleOrDefaultAsync(new GetOptions { Filters = { 
                    { "OwnerId", data.OwnerId},
                    { "Name", data.Name }
                } });

            if (existent != null)
                throw new ACompanyForThatNameAlreadyExistException();

            return data;
        }
    }
}
