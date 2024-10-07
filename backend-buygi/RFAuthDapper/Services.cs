using Microsoft.Extensions.DependencyInjection;
using RFAuth.IRepo;
using RFAuthDapper.Dapper;

namespace RFAuthDapper
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFAuthDapper(this IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserDapper>();
            services.AddScoped<IPasswordRepo, PasswordDapper>();
            services.AddScoped<IDeviceRepo, DeviceDapper>();
            services.AddScoped<ISessionRepo, SessionDapper>();
        }
    }
}
