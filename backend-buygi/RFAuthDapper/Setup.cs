using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using System.Data;

namespace RFAuthDapper
{
    public static class Setup
    {
        public static void ConfigureRFAuthDapper(IServiceProvider services)
        {
            var connection = services.GetService<IDbConnection>() ??
                throw new Exception("No connection available");

            (new Devices(connection)).CreateTable();
            (new UsersTypes(connection)).CreateTable();
            (new Users(connection)).CreateTable();
            (new Passwords(connection)).CreateTable();
            (new Sessions(connection)).CreateTable();
        }
    }
}
