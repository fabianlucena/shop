using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using backend_shopia.Services;
using NetTopologySuite.Geometries;
using Npgsql;
using RFAuth;
using RFAuthDapper;
using RFDapper;
using RFDapperDriverPostgreSQL;
using RFDBLocalizer;
using RFDBLocalizer.IServices;
using RFDBLocalizerDapper;
using RFHttpAction;
using RFHttpActionDapper;
using RFHttpExceptionsL10n;
using RFL10n;
using RFLogger;
using RFLoggerProvider;
using RFLoggerProviderDapper;
using RFRBAC;
using RFRBAC.Authorization;
using RFRBACDapper;
using RFRegister;
using RFService;
using RFService.Attributes;
using RFService.IRepo;
using RFUserEmailVerified;
using RFUserEmailVerifiedDapper;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace backend_shopia
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

            services.AddControllers(options => options.Filters.Add<RBACFilter>());

            services.AddRFService();
            services.AddRFLogger();
            services.AddRFLoggerProvider();
            services.AddRFL10n();
            services.AddRFAuth();
            services.AddRFUserEmailVerified();
            services.AddRFRBAC();
            services.AddRFRegister();
            services.AddRFHttpAction();
            services.AddRFHttpExceptionsL10n();
            services.AddRFDBLocalizer();

            services.AddSingleton<IEmbeddingService>(provider =>
            {
                var url = builder.Configuration.GetValue<string>("Embedding:Url")
                    ?? throw new Exception("No Embedding:Url configuration found");
                var apiKey = builder.Configuration.GetValue<string>("Embedding:ApiKey") ?? string.Empty;
                var model = builder.Configuration.GetValue<string>("Embedding:Model")
                    ?? throw new Exception("No Embedding:Model configuration found");
                return new EmbeddingService(url, apiKey, model);
            });

            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IPlanLimitService, PlanLimitService>();
            services.AddScoped<IUserPlanService, UserPlanService>();
            services.AddScoped<ICommerceService, CommerceService>();
            services.AddScoped<ICommerceFileService, CommerceFileService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemFileService, ItemFileService>();

            services.AddRFLoggerProviderDapper();
            services.AddRFAuthDapper();
            services.AddRFUserEmailVerifiedDapper();
            services.AddRFRBACDapper();
            services.AddRFHttpActionDapper();
            services.AddRFDBLocalizerDapper();

            services.AddScoped<Dapper<Plan>, Dapper<Plan>>();
            services.AddScoped<Dapper<PlanLimit>, Dapper<PlanLimit>>();
            services.AddScoped<Dapper<UserPlan>, Dapper<UserPlan>>();
            services.AddScoped<Dapper<Commerce>, Dapper<Commerce>>();
            services.AddScoped<Dapper<CommerceFile>, Dapper<CommerceFile>>();
            services.AddScoped<Dapper<Store>, Dapper<Store>>();
            services.AddScoped<Dapper<Category>, Dapper<Category>>();
            services.AddScoped<Dapper<Item>, Dapper<Item>>();
            services.AddScoped<Dapper<ItemFile>, Dapper<ItemFile>>();

            services.AddScoped<IRepo<Plan>, Dapper<Plan>>();
            services.AddScoped<IRepo<PlanLimit>, Dapper<PlanLimit>>();
            services.AddScoped<IRepo<UserPlan>, Dapper<UserPlan>>();
            services.AddScoped<IRepo<Commerce>, Dapper<Commerce>>();
            services.AddScoped<IRepo<CommerceFile>, Dapper<CommerceFile>>();
            services.AddScoped<IRepo<Store>, Dapper<Store>>();
            services.AddScoped<IRepo<Category>, Dapper<Category>>();
            services.AddScoped<IRepo<Item>, Dapper<Item>>();
            services.AddScoped<IRepo<ItemFile>, Dapper<ItemFile>>();

            services.AddRFDapperDriverPostgreSQL(new PostgreSQLDDOptions
            {
                ConnectionString = dbConnectionString,
                OpenConnection = (driverOptions, connectionString) => {
                    connectionString ??= driverOptions.ConnectionString;
                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

                    var dsb = new NpgsqlDataSourceBuilder(connectionString);
                    dsb.UseVector();
                    var dataSource = dsb.Build();
                    var connection = dataSource.OpenConnection();

                    return connection;
                },
                ColumnTypes =
                {
                    { "Point", property => "GEOGRAPHY(Point, 4326)" },
                    { "Single[]",  property => {
                        var length = property.GetCustomAttribute<MaxLengthAttribute>()?.Length
                            ?? property.GetCustomAttribute<LengthAttribute>()?.MaximumLength
                            ?? property.GetCustomAttribute<SizeAttribute>()?.Size;

                        if (!length.HasValue || length.Value <= 0)
                            throw new MissingLengthForPropertyException(property.Name);
                        
                        return $"Vector({length.Value})";
                    } },
                },
                GetSqlSelectedProperty = (driver, property, options, defaultAlias) =>
                {
                    if (property.PropertyType == typeof(Point))
                        return $"ST_AsText({driver.GetColumnName(property.Name, options, defaultAlias)}) AS {driver.GetColumnAlias(property.Name)}";

                    return null;
                },
            });

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        public static void ConfigureTranslations(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool?>("Translate") == false)
                return;

            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            RFAuth_es.Setup.ConfigureDataRFAuthEs(serviceProvider);
            RFDapper_es.Setup.ConfigureDataRFDapperEs(serviceProvider);
            RFService_es.Setup.ConfigureDataRFServiceEs(serviceProvider);

            var l10n = serviceProvider.GetRequiredService<IL10n>();
            var translator = serviceProvider.GetRequiredService<IDBTranslator>();

            l10n.AddTranslationsFromFile("es", "", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translations\\es.txt"));
            l10n.AddTranslationsFromFile("es", "exception", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translations\\exception_es.txt"));
        }

        public static void ConfigureRepo(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("CreateDapperTables"))
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                RFAuthDapper.Setup.ConfigureRFAuthDapper(serviceProvider);
                RFLoggerProviderDapper.Setup.ConfigureRFLoggerProviderDapper(serviceProvider);
                RFUserEmailVerifiedDapper.Setup.ConfigureRFUserEmailVerifiedDapper(serviceProvider);
                RFRBACDapper.Setup.ConfigureRFRBACDapper(serviceProvider);
                RFHttpActionDapper.Setup.ConfigureRFHttpActionDapper(serviceProvider);
                Setup.ConfigureShopiaDapper(serviceProvider);
            }
        }

        public static void ConfigureData(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("UpdateData"))
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                RFAuth.Setup.ConfigureRFAuth(serviceProvider);
                RFAuth.Setup.ConfigureDataRFAuth(serviceProvider);
                RFUserEmailVerified.Setup.ConfigureRFUserEmailVerified(serviceProvider);
                RFRBAC.Setup.ConfigureRFRBAC(serviceProvider);
                RFRegister.Setup.ConfigureRFRegister(serviceProvider);
                Setup.ConfigureShopia(serviceProvider);
            }
        }
    }
}