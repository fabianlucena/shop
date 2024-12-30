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

        static IRolePermissionService RolePermissionService => rolePermissionService ?? throw new Exception();
        static IPlanService PlanService => planService ?? throw new Exception();

        public static void ConfigureShopDapper(IServiceProvider services)
        {
            CreateTable<Plan>(services);
            CreateTable<UserPlan>(services);
            CreateTable<Business>(services);
        }

        public static void ConfigureShop(IServiceProvider provider)
        {
            rolePermissionService = provider.GetRequiredService<IRolePermissionService>();
            planService = provider.GetRequiredService<IPlanService>();

            ConfigureShopAsync().Wait();
        }

        public static async Task ConfigureShopAsync()
        {
            var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                { "user", [
                    "changePassword",
                    "business.get", "business.add", "business.edit", "business.delete",
                ] },
            };

            await RolePermissionService.AddRolesPermissionsAsync(rolesPermissions);

            await PlanService.GetOrCreateAsync(new Plan {
                Name = "Base",
                Description = "Plan básico para todos los ususarios",
                MaxEnabledBusiness = 1,
            });
        }
    }
}
