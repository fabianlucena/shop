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
                
                

                await addTranslationService.AddAsync("es", "exception", "Business does not exist.", "El negocio no existe.");
                await addTranslationService.AddAsync("es", "exception", "Categorys does not exist.", "El rubro no existe.");
                await addTranslationService.AddAsync("es", "exception", "Item does not exist.", "No existe el artículo.");
                await addTranslationService.AddAsync("es", "exception", "No business provided.", "No se ha provisto negocio.");
                await addTranslationService.AddAsync("es", "exception", "No category provided.", "No se ha provisto una categoría.");
                await addTranslationService.AddAsync("es", "exception", "No name for business provided.", "No se ha provisto un nombre para el negocio.");
                await addTranslationService.AddAsync("es", "exception", "No name provided.", "No se ha provisto un nombre.");
                await addTranslationService.AddAsync("es", "exception", "No owner provided.", "No se ha provisto propietario.");
                await addTranslationService.AddAsync("es", "exception", "No plan provided.", "No se ha provisto plan.");
                await addTranslationService.AddAsync("es", "exception", "No store provided.", "No se ha provisto un local.");
                await addTranslationService.AddAsync("es", "exception", "Plan already exists.", "El plan ya existe.");
                await addTranslationService.AddAsync("es", "exception", "Plan feature already exists.", "La característica de plan ya existe.");
                await addTranslationService.AddAsync("es", "exception", "Store does not exist.", "No existe el local.");
                await addTranslationService.AddAsync("es", "exception", "The maximum limit of enabled businesses has been reached.", "Se ha alcanzado el límite máximo de empresas habilitadas.");
                await addTranslationService.AddAsync("es", "exception", "The maximum limit of enabled items has been reached.", "Se ha alcanzado el límite máximo de artículos habilitadas.");
                await addTranslationService.AddAsync("es", "exception", "The maximum limit of enabled stores has been reached.", "Se ha alcanzado el límite máximo de locales habilitados.");
                await addTranslationService.AddAsync("es", "exception", "You already own a business with that name.", "Uste ya posee un negocio con ese nombre.");
                await addTranslationService.AddAsync("es", "exception", "You already own a store with that name.", "Uste ya posee un local con ese nombre.");
                await addTranslationService.AddAsync("es", "exception", "You can't create more businesses because you've reached the limit.", "Usted no puede crear más negocios porque ha alcanzado el límite.");
                await addTranslationService.AddAsync("es", "exception", "You can't create more items because you've reached the limit.", "Usted no puede crear más artículos porque ha alcanzado el límite.");
                await addTranslationService.AddAsync("es", "exception", "You can't create more stores because you've reached the limit.", "Usted no puede crear más locales porque ha alcanzado el límite.");
            }
        }
    }
}
