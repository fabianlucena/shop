using RFLocalizer;
using RFAuth;
using RFAuthDapper;
using RFRBAC;
using RFRBACDapper;
using RFRBAC.Authorization;
using RFRegister;
using RFUserEmailVerified;
using RFHttpAction;
using RFHttpActionDapper;
using RFDapperDriverSQLServer;
using RFUserEmailVerifiedDapper;
using backend_shop.IServices;
using RFDapper;
using backend_shop.Entities;
using RFService.IRepo;
using static RFDapper.Setup;
using backend_shop.Service;

namespace backend_shop
{
    public static class MvcServiceCollectionExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            
            services.AddCors(options =>
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

            RFDapper.Setup.ConfigureDefaultDBConnectionString(dbConnectionString);

            services.AddControllers(options => options.Filters.Add<RBACFilter>());

            services.AddRFLocalizer();
            services.AddRFAuth();
            services.AddRFUserEmailVerified();
            services.AddRFRBAC();
            services.AddRFRegister();
            services.AddRFHttpAction();

            services.AddScoped<ICompanyService, CompanyService>();

            services.AddRFAuthDapper();
            services.AddRFUserEmailVerifiedDapper();
            services.AddRFRBACDapper();
            services.AddRFHttpActionDapper();

            services.AddScoped<Dapper<Company>, Dapper<Company>>();

            services.AddScoped<IRepo<Company>, Dapper<Company>>();

            services.AddRFDapperDriverSQLServer();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        public static void ConfigureRepo(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("CreateDapperTables"))
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                RFAuthDapper.Setup.ConfigureRFAuthDapper(serviceProvider);
                RFUserEmailVerifiedDapper.Setup.ConfigureRFUserEmailVerifiedDapper(serviceProvider);
                RFRBACDapper.Setup.ConfigureRFRBACDapper(serviceProvider);
                RFHttpActionDapper.Setup.ConfigureRFHttpActionDapper(serviceProvider);

                CreateTable<Company>(serviceProvider);
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
                RFRBAC.Setup.ConfigureRFRBAC(serviceProvider);
                RFRegister.Setup.ConfigureRFRegister(serviceProvider);
            }
        }
    }
}