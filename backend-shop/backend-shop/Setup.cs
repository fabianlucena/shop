using backend_shop.Entities;
using backend_shop.IServices;
using RFRBAC.IServices;
using static RFDapper.Setup;

namespace backend_shop
{
    public static class Setup
    {
        static IRolePermissionService? rolePermissionService;
        static IPlanService? planService;
        static ICategoryService? categoryService;

        static IRolePermissionService RolePermissionService => rolePermissionService ?? throw new Exception();
        static IPlanService PlanService => planService ?? throw new Exception();
        static ICategoryService CategoryService => categoryService ?? throw new Exception();

        public static void ConfigureShopDapper(IServiceProvider services)
        {
            CreateTable<Plan>(services);
            CreateTable<UserPlan>(services);
            CreateTable<Business>(services);
            CreateTable<Store>(services);
            CreateTable<Category>(services);
            CreateTable<Item>(services);
        }

        public static void ConfigureShop(IServiceProvider provider)
        {
            rolePermissionService = provider.GetRequiredService<IRolePermissionService>();
            planService = provider.GetRequiredService<IPlanService>();
            categoryService = provider.GetRequiredService<ICategoryService>();

            ConfigureShopAsync().Wait();
        }

        public static async Task ConfigureShopAsync()
        {
            var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                { "user", [
                    "changePassword",
                    "business.get", "business.add", "business.edit", "business.delete", "business.restore",
                    "store.get", "store.add", "store.edit", "store.delete", "store.restore",
                ] },
            };

            await RolePermissionService.AddRolesPermissionsAsync(rolesPermissions);

            await PlanService.GetOrCreateAsync(new Plan {
                Name = "Base",
                Description = "Plan básico para todos los ususarios",
                MaxTotalBusinesses = 3,
                MaxEnabledBusinesses = 1,
                MaxTotalStores = 5,
                MaxEnabledStores = 3,
                MaxTotalItems = 15,
                MaxEnabledItems = 10,
            });

            var categories = new Dictionary<string, string>{
                { "Almacén",      "Artículos de almacén, comestibles, bebidas." },
                { "Indumentaria", "Ropas y vestimenta en general." },
            };

            foreach (var category in categories)
            {
                await CategoryService.GetOrCreateAsync(new Category { Name = category.Key, Description = category.Value });
            }
        }
    }
}
