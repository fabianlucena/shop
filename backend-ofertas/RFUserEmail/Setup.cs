using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFService.IService;
using RFUserEmail.IServices;

namespace RFUserEmail
{
    public static class Setup
    {
        public static void ConfigureRFUserEmail(IServiceProvider provider)
        {
            var propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();
            var userEmailService = provider.GetRequiredService<IUserEmailService>();
            propertiesDecorators.AddDecorator("LoginAttributes", async (data, property) => {
                var userEmail = await userEmailService.GetSingleOrDefaultAsyncForUserId(((LoginData)data).UserId);
                if (userEmail == null)
                    property["HasEmail"] = false;
                else
                {
                    property["HasEmail"] = true;
                    property["IsEmailVerified"] = userEmail.IsVerified;
                }
            });
        }
    }
}
