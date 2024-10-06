using AutoMapper;
using backend_buygi;
using backend_buygi.Dapper;
using backend_buygi.Repo;
using backend_buygi.IServices;
using backend_buygi.Services;
using Microsoft.Data.SqlClient;
using System.Data;

public static class MvcServiceCollectionExtensions
{
    public static void ConfigureDependencies(this WebApplicationBuilder builder)
    {
        string dbConnectionString = builder.Configuration.GetConnectionString("dbConnection")
            ?? throw new Exception("No DB connection founded, try adding a dbConnection property to ConnectionStrings on appsettings.json");

        var services = builder.Services;

        services.AddSingleton<IDbConnection>(sp => {
            var connection = new SqlConnection(dbConnectionString);
            connection.Open();
            return connection;
        });

        services.AddScoped<IUserRepo, UserDapper>();
        services.AddScoped<IPasswordRepo, PasswordDapper>();
        services.AddScoped<IDeviceRepo, DeviceDapper>();
        services.AddScoped<ISessionRepo, SessionDapper>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ILoginService, LoginService>();

        var mapperConfig = new MapperConfiguration(m => {
            m.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();

        services.AddSingleton(mapper);
    }
}