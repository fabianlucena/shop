using Microsoft.Extensions.DependencyInjection;
using RFAuth.IRepo;

namespace RFAuthDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFAuthDapper(this IServiceCollection services)
        {
            services.AddScoped<IUserTypeRepo, UsersTypes>();
            services.AddScoped<IUserRepo, Users>();
            services.AddScoped<IPasswordRepo, Passwords>();
            services.AddScoped<IDeviceRepo, Devices>();
            services.AddScoped<ISessionRepo, Sessions>();
        }
    }
}
