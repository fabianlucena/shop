using Microsoft.Extensions.DependencyInjection;
using RFLocalizer.IServices;

namespace backend_shop_es
{
    public static class Setup
    {
        public static void ConfigureShopEs(IServiceProvider provider)
            => ConfigureShopEsAsync(provider).Wait();

        public static async Task ConfigureShopEsAsync(IServiceProvider provider)
        {
            var addTranslationService = provider.GetService<IAddTranslationService>();
            if (addTranslationService != null)
            {
                await addTranslationService.AddAsync("es", "exception", "Commerce does not exist.", "El comercio no existe.");
                await addTranslationService.AddAsync("es", "exception", "Categorys does not exist.", "El rubro no existe.");
                await addTranslationService.AddAsync("es", "exception", "Item does not exist.", "No existe el artículo.");
                await addTranslationService.AddAsync("es", "exception", "No commerce provided.", "No se ha provisto comercio.");
                await addTranslationService.AddAsync("es", "exception", "No category provided.", "No se ha provisto una categoría.");
                await addTranslationService.AddAsync("es", "exception", "No name for commerce provided.", "No se ha provisto un nombre para el comercio.");
                await addTranslationService.AddAsync("es", "exception", "No name provided.", "No se ha provisto un nombre.");
                await addTranslationService.AddAsync("es", "exception", "No owner provided.", "No se ha provisto propietario.");
                await addTranslationService.AddAsync("es", "exception", "No plan provided.", "No se ha provisto plan.");
                await addTranslationService.AddAsync("es", "exception", "No store provided.", "No se ha provisto un local.");
                await addTranslationService.AddAsync("es", "exception", "Plan already exists.", "El plan ya existe.");
                await addTranslationService.AddAsync("es", "exception", "Plan feature already exists.", "La característica de plan ya existe.");
                await addTranslationService.AddAsync("es", "exception", "Store does not exist.", "No existe el local.");
                await addTranslationService.AddAsync("es", "exception", "The maximum limit of enabled commerces has been reached.", "Se ha alcanzado el límite máximo de comercios habilitadas.");
                await addTranslationService.AddAsync("es", "exception", "The maximum limit of enabled items has been reached.", "Se ha alcanzado el límite máximo de artículos habilitadas.");
                await addTranslationService.AddAsync("es", "exception", "The maximum limit of enabled stores has been reached.", "Se ha alcanzado el límite máximo de locales habilitados.");
                await addTranslationService.AddAsync("es", "exception", "You already own a commerce with that name.", "Uste ya posee un comercio con ese nombre.");
                await addTranslationService.AddAsync("es", "exception", "You already own a store with that name.", "Uste ya posee un local con ese nombre.");
                await addTranslationService.AddAsync("es", "exception", "You can't create more commerces because you've reached the limit.", "Usted no puede crear más comercios porque ha alcanzado el límite.");
                await addTranslationService.AddAsync("es", "exception", "You can't create more items because you've reached the limit.", "Usted no puede crear más artículos porque ha alcanzado el límite.");
                await addTranslationService.AddAsync("es", "exception", "You can't create more stores because you've reached the limit.", "Usted no puede crear más locales porque ha alcanzado el límite.");
            }
        }
    }
}
