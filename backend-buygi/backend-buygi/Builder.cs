using Microsoft.Data.SqlClient;
using RFAuth;
using RFAuthDapper;
using RFRegister;
using RFUserEmail;
using RFUserEmailDapper;
using System.Data;

namespace backend_buygi
{
    public static class MvcServiceCollectionExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            string dbConnectionString = builder.Configuration.GetConnectionString("dbConnection")
                ?? throw new Exception("No DB connection founded, try adding a dbConnection property to ConnectionStrings on appsettings.json");

            var services = builder.Services;

            services.AddSingleton<IDbConnection>(sp =>
            {
                var connection = new SqlConnection(dbConnectionString);
                connection.Open();
                return connection;
            });

            services.AddRFAuth();
            services.AddRFUserEmail();
            services.AddRFRegister();

            services.AddRFAuthDapper();
            services.AddRFUserEmailDapper();

            if (builder.Configuration.GetValue<bool>("CreateDapperTables"))
            {
                using var connection = new SqlConnection(dbConnectionString);
                connection.Open();

                RFAuthDapper.Setup.ConfigureRFAuthDapper(connection);
                RFUserEmailDapper.Setup.ConfigureRFUserEmailDapper(connection);
            }
        }

        public static void ConfigureData(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("UpdateData"))
            {
                using var scope = (app.Services.CreateScope());
                var serviceProvider = scope.ServiceProvider;

                RFAuth.Setup.ConfigureRFAuthAsync(serviceProvider).Wait();
                RFUserEmail.Setup.ConfigureRFUserEmail(serviceProvider).Wait();
                RFRegister.Setup.ConfigureRFRegister(serviceProvider).Wait();
            }
        }
    }
}