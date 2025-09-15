using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Service;
using RFRBAC.IServices;
using static RFDapper.Setup;

namespace backend_shop
{
    public static class Setup
    {
        static IRolePermissionService? rolePermissionService;
        static IPlanService? planService;
        static IPlanLimitService? planLimitService;
        static ICategoryService? categoryService;

        static IRolePermissionService RolePermissionService => rolePermissionService ?? throw new Exception();
        static IPlanService PlanService => planService ?? throw new Exception();
        static IPlanLimitService PlanLimitService => planLimitService ?? throw new Exception();
        static ICategoryService CategoryService => categoryService ?? throw new Exception();

        public static void ConfigureShopDapper(IServiceProvider services)
        {
            CreateTable<Plan>(services);
            CreateTable<PlanLimit>(services);
            CreateTable<UserPlan>(services);
            CreateTable<Commerce>(services);
            CreateTable<CommerceFile>(services);
            CreateTable<Store>(services);
            CreateTable<Category>(services);
            CreateTable<Item>(services);
            CreateTable<ItemFile>(services);
        }

        public static void ConfigureShop(IServiceProvider provider)
        {
            rolePermissionService = provider.GetRequiredService<IRolePermissionService>();
            planService = provider.GetRequiredService<IPlanService>();
            planLimitService = provider.GetRequiredService<IPlanLimitService>();
            categoryService = provider.GetRequiredService<ICategoryService>();

            ConfigureShopAsync().Wait();
        }

        public static async Task ConfigureShopAsync()
        {
            var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                { "user", [
                    "changePassword",
                    "plan.get",
                    "commerce.get", "commerce.add", "commerce.edit", "commerce.delete", "commerce.restore",
                    "store.get", "store.add", "store.edit", "store.delete", "store.restore",
                    "category.get",
                    "item.get", "item.add", "item.edit", "item.delete", "item.restore",
                ] },
            };

            await RolePermissionService.AddRolesPermissionsAsync(rolesPermissions);

            var basePlan = await PlanService.GetOrCreateAsync(new Plan {
                Name = "Base",
                Description = "Plan básico para todos los ususarios",
            });
            var basePlanId = basePlan.Id;

            var limits = new List<PlanLimit>{
                new () { Name = "MaxTotalCommerces", Limit = 3 },
                new () { Name = "MaxEnabledCommerces", Limit = 1 },
                new () { Name = "MaxCommerceImageSize", Limit = 1000000 },
                new () { Name = "MaxTotalImagesPerSingleCommerce", Limit = 3 },
                new () { Name = "MaxTotalCommercesImages", Limit = 5 },
                new () { Name = "MaxEnabledCommercesImages", Limit = 3 },
                new () { Name = "MaxCommercesImagesAggregatedSize", Limit = 5000000 },
                new () { Name = "MaxEnabledCommercesImagesAggregatedSize", Limit = 5000000 },
                new () { Name = "MaxTotalStores", Limit = 5 },
                new () { Name = "MaxEnabledStores", Limit = 3 },
                new () { Name = "MaxTotalItems", Limit = 15 },
                new () { Name = "MaxEnabledItems", Limit = 10 },
                new () { Name = "MaxItemImageSize", Limit = 1000000 },
                new () { Name = "MaxTotalImagesPerSingleItem", Limit = 3 },
                new () { Name = "MaxTotalItemsImages", Limit = 8 },
                new () { Name = "MaxEnabledItemsImages", Limit = 6 },
                new () { Name = "MaxItemsImagesAggregatedSize", Limit = 10000000 },
                new () { Name = "MaxEnabledItemsImagesAggregatedSize", Limit = 10000000 },
            };

            foreach (var limit in limits)
            {
                limit.PlanId = basePlanId;
                await PlanLimitService.CreateAsync(limit);
            }

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
