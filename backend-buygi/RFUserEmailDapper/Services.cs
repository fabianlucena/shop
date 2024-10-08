using Microsoft.Extensions.DependencyInjection;
using RFUserEmail.IRepo;
using RFUserEmailDapper.Tables;

namespace RFUserEmailDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFUserEmailDapper(this IServiceCollection services)
        {
            services.AddScoped<IUserEmailRepo, UsersEmails>();
        }
    }
}
