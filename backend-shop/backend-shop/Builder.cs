﻿using RFLocalizer;
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
using backend_shop.Service;
using RFLocalizerDapper;

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

            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IUserPlanService, UserPlanService>();
            services.AddScoped<ICommerceService, CommerceService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IItemService, ItemService>();

            services.AddRFLocalizerDapper();
            services.AddRFAuthDapper();
            services.AddRFUserEmailVerifiedDapper();
            services.AddRFRBACDapper();
            services.AddRFHttpActionDapper();

            services.AddScoped<Dapper<Plan>, Dapper<Plan>>();
            services.AddScoped<Dapper<UserPlan>, Dapper<UserPlan>>();
            services.AddScoped<Dapper<Commerce>, Dapper<Commerce>>();
            services.AddScoped<Dapper<Store>, Dapper<Store>>();
            services.AddScoped<Dapper<Category>, Dapper<Category>>();
            services.AddScoped<Dapper<Item>, Dapper<Item>>();

            services.AddScoped<IRepo<Plan>, Dapper<Plan>>();
            services.AddScoped<IRepo<UserPlan>, Dapper<UserPlan>>();
            services.AddScoped<IRepo<Commerce>, Dapper<Commerce>>();
            services.AddScoped<IRepo<Store>, Dapper<Store>>();
            services.AddScoped<IRepo<Category>, Dapper<Category>>();
            services.AddScoped<IRepo<Item>, Dapper<Item>>();

            services.AddRFDapperDriverSQLServer();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        public static void ConfigureRepo(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("CreateDapperTables"))
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                RFLocalizerDapper.Setup.ConfigureRFLocalizerDapper(serviceProvider);
                RFAuthDapper.Setup.ConfigureRFAuthDapper(serviceProvider);
                RFUserEmailVerifiedDapper.Setup.ConfigureRFUserEmailVerifiedDapper(serviceProvider);
                RFRBACDapper.Setup.ConfigureRFRBACDapper(serviceProvider);
                RFHttpActionDapper.Setup.ConfigureRFHttpActionDapper(serviceProvider);
                Setup.ConfigureShopDapper(serviceProvider);
            }
        }

        public static void ConfigureData(this WebApplication app)
        {
            if (app.Configuration.GetValue<bool>("UpdateData"))
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                backend_shop_es.Setup.ConfigureShopEs(serviceProvider);
                RFAuth.Setup.ConfigureRFAuth(serviceProvider);
                RFUserEmailVerified.Setup.ConfigureRFUserEmailVerified(serviceProvider);
                RFRBAC.Setup.ConfigureRFRBAC(serviceProvider);
                RFRegister.Setup.ConfigureRFRegister(serviceProvider);
                Setup.ConfigureShop(serviceProvider);
            }
        }
    }
}