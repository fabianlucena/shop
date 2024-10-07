using Microsoft.Extensions.DependencyInjection;
using RFUserEmail.IRepo;
using RFUserEmailDapper.Dapper;

namespace RFUserEmailDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFUserEmailDapper(this IServiceCollection services)
        {
            services.AddScoped<IUserEmailRepo, UserEmailDapper>();
        }
    }
}
