using Microsoft.Extensions.DependencyInjection;
using RFAuth.IRepo;
using RFAuthDapper.Tables;

namespace RFAuthDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFAuthDapper(this IServiceCollection services)
        {
            services.AddScoped<IUserRepo, Users>();
            services.AddScoped<IPasswordRepo, Passwords>();
            services.AddScoped<IDeviceRepo, Devices>();
            services.AddScoped<ISessionRepo, Sessions>();
        }
    }
}
