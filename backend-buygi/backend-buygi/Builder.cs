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
            services.AddRFAuthDapper();
            services.AddRFUserEmail();
            services.AddRFUserEmailDapper();
            services.AddRFRegister();

            if (builder.Configuration.GetValue<bool>("CreateDapperTables")) {
                using var connection = new SqlConnection(dbConnectionString);
                connection.Open();

                ConfigureRFAuthDapper.Setup(connection);
                ConfigureRFUserEmailDapper.Setup(connection);
            }
        }
    }
}