using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace RFUserEmailDapper
{
    public static class Setup
    {
        public static void ConfigureRFUserEmailDapper(IServiceProvider services)
        {
            var connection = services.GetService<IDbConnection>() ??
                throw new Exception("No connection available");

            (new UsersEmails(connection)).CreateTable();
        }
    }
}