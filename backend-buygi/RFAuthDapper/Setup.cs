using System.Data;

namespace RFAuthDapper
{
    public static class Setup
    {
        public static void ConfigureRFAuthDapper(IDbConnection connection)
        {
            (new Devices(connection)).CreateTable();
            (new UsersTypes(connection)).CreateTable();
            (new Users(connection)).CreateTable();
            (new Passwords(connection)).CreateTable();
            (new Sessions(connection)).CreateTable();
        }
    }
}
