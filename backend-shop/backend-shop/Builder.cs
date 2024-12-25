using Microsoft.Data.SqlClient;
using RFAuth;
using RFAuthDapper;
using RFRBAC.Authorization;
using RFRegister;
using RFUserEmail;
using RFUserEmailDapper;
using RFHttpAction;
using RFHttpActionDapper;
using System.Data;
using RFUserEmailVerified;

namespace backend_shop
{
    public static class MvcServiceCollectionExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("allowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()    // Permite cualquier origen (no recomendable en producción)
                               .AllowAnyMethod()    // Permite cualquier método (GET, POST, PUT, DELETE, etc.)
                               .AllowAnyHeader();   // Permite cualquier encabezado
                    });
            });

            string dbConnectionString = builder.Configuration.GetConnectionString("dbConnection")
                ?? throw new Exception("No DB connection founded, try adding a dbConnection property to ConnectionStrings on appsettings.json");

            var services = builder.Services;

            services.AddSingleton<IDbConnection>(sp =>
            {
                var connection = new SqlConnection(dbConnectionString);
                connection.Open();
                return connection;
            });

            services.AddControllers(options => options.Filters.Add<RBACFilter>());

            services.AddRFAuth();
            services.AddRFUserEmail();
            services.AddRFRegister();
            services.AddRFHttpAction();

            services.AddRFAuthDapper();
            services.AddRFUserEmailDapper();
            services.AddRFHttpActionDapper();
        }

        public static void ConfigureRepo(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("CreateDapperTables"))
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                RFAuthDapper.Setup.ConfigureRFAuthDapper(serviceProvider);
                RFUserEmailDapper.Setup.ConfigureRFUserEmailDapper(serviceProvider);
                RFHttpActionDapper.Setup.ConfigureRFHttpActionDapper(serviceProvider);
            }
        }

        public static void ConfigureData(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("UpdateData"))
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                RFAuth.Setup.ConfigureRFAuth(serviceProvider);
                RFUserEmailVerified.Setup.ConfigureRFUserEmailVerified(serviceProvider);
                RFRegister.Setup.ConfigureRFRegister(serviceProvider);
            }
        }
    }
}