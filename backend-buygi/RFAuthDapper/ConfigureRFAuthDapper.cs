using RFAuthDapper.Dapper;
using System.Data;

namespace RFAuthDapper
{
    public static class ConfigureRFAuthDapper
    {
        public static void Setup(IDbConnection connection)
        {
            (new DeviceDapper(connection)).CreateTable();
            (new UserDapper(connection)).CreateTable();
            (new PasswordDapper(connection)).CreateTable();
            (new SessionDapper(connection)).CreateTable();
        }
    }
}
