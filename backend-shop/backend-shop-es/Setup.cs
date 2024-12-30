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
                await addTranslationService.AddAsync("es", "exception", "You can't create more businesses because you've reached the limit.", "Usted no puede crear más negocios porque ha alcanzado el límite.");
                await addTranslationService.AddAsync("es", "exception", "You already own a business with that name.", "Uste ya posee un negocio con ese nombre.");
                await addTranslationService.AddAsync("es", "exception", "No name provided.", "No se ha provisto un nombre.");
                await addTranslationService.AddAsync("es", "exception", "No name for business provided.", "No se ha provisto un nombre para el negocio.");
                await addTranslationService.AddAsync("es", "exception", "No owner provided.", "No se ha provisto propietario.");
                await addTranslationService.AddAsync("es", "exception", "No plan provided.", "No se ha provisto plan.");
                await addTranslationService.AddAsync("es", "exception", "Plan already exists.", "El plan ya existe.");
                await addTranslationService.AddAsync("es", "exception", "Plan feature already exists.", "La característica de plan ya existe.");
                await addTranslationService.AddAsync("es", "exception", "Business does not exist.", "No existe el negocio.");
            }
        }
    }
}
