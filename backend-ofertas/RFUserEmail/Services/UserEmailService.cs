using RFService.IRepo;
using RFService.RepoLib;
using RFService.ServicesLib;
using RFUserEmail.Entities;
using RFUserEmail.IServices;

namespace RFUserEmail.Services
{
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    public class UserEmailService(IRepo<UserEmail> repo) : ServiceTimestampsIdUuid<IRepo<UserEmail>, UserEmail>(repo), IUserEmailService
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    {
        public override async Task<UserEmail> ValidateForCreationAsync(UserEmail data)
        {
            data = await base.ValidateForCreationAsync(data);
            data.IsVerified ??= false;

            return data;
        }

        public Task<UserEmail?> GetSingleOrDefaultAsyncForUserId(Int64 userId)
        {
            return repo.GetSingleOrDefaultAsync(new GetOptions { Filters = { { "UserId", userId } } } );
        }
    }
}
