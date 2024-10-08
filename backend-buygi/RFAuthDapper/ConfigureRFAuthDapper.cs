using RFAuthDapper.Tables;
using System.Data;

namespace RFAuthDapper
{
    public static class ConfigureRFAuthDapper
    {
        public static void Setup(IDbConnection connection)
        {
            (new Devices(connection)).CreateTable();
            (new Users(connection)).CreateTable();
            (new Passwords(connection)).CreateTable();
            (new Sessions(connection)).CreateTable();
        }
    }
}
