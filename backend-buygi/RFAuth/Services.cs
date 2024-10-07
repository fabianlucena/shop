using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFAuth.Services;

namespace RFAuth
{
    public static class MvcServiceCollectionExtensions
    {
        public static void AddRFAuth(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ILoginService, LoginService>();

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}