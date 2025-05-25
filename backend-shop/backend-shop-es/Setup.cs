using Microsoft.Extensions.DependencyInjection;
using RFL10n;

namespace backend_shop_es
{
    public static class Setup
    {
        public static void ConfigureShopEs(IServiceProvider provider)
        {
            var i10n = provider.GetService<IL10n>();
            if (i10n != null)
            {
                i10n.AddTranslation("es", "exception", "Commerce does not exist.", "El comercio no existe.");
                i10n.AddTranslation("es", "exception", "Categorys does not exist.", "El rubro no existe.");
                i10n.AddTranslation("es", "exception", "Item does not exist.", "No existe el artículo.");
                i10n.AddTranslation("es", "exception", "No commerce provided.", "No se ha provisto comercio.");
                i10n.AddTranslation("es", "exception", "No category provided.", "No se ha provisto una categoría.");
                i10n.AddTranslation("es", "exception", "No name for commerce provided.", "No se ha provisto un nombre para el comercio.");
                i10n.AddTranslation("es", "exception", "No name provided.", "No se ha provisto un nombre.");
                i10n.AddTranslation("es", "exception", "No owner provided.", "No se ha provisto propietario.");
                i10n.AddTranslation("es", "exception", "No plan provided.", "No se ha provisto plan.");
                i10n.AddTranslation("es", "exception", "No store provided.", "No se ha provisto un local.");
                i10n.AddTranslation("es", "exception", "Plan already exists.", "El plan ya existe.");
                i10n.AddTranslation("es", "exception", "Plan feature already exists.", "La característica de plan ya existe.");
                i10n.AddTranslation("es", "exception", "Store does not exist.", "No existe el local.");
                i10n.AddTranslation("es", "exception", "The maximum limit of enabled commerces has been reached.", "Se ha alcanzado el límite máximo de comercios habilitadas.");
                i10n.AddTranslation("es", "exception", "The maximum limit of enabled items has been reached.", "Se ha alcanzado el límite máximo de artículos habilitadas.");
                i10n.AddTranslation("es", "exception", "The maximum limit of enabled stores has been reached.", "Se ha alcanzado el límite máximo de locales habilitados.");
                i10n.AddTranslation("es", "exception", "You already own a commerce with that name.", "Uste ya posee un comercio con ese nombre.");
                i10n.AddTranslation("es", "exception", "You already own a store with that name.", "Uste ya posee un local con ese nombre.");
                i10n.AddTranslation("es", "exception", "You can't create more commerces because you've reached the limit.", "Usted no puede crear más comercios porque ha alcanzado el límite.");
                i10n.AddTranslation("es", "exception", "You can't create more items because you've reached the limit.", "Usted no puede crear más artículos porque ha alcanzado el límite.");
                i10n.AddTranslation("es", "exception", "You can't create more stores because you've reached the limit.", "Usted no puede crear más locales porque ha alcanzado el límite.");
            }
        }
    }
}
