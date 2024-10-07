using RFUserEmailDapper.Dapper;
using System.Data;

namespace RFUserEmailDapper
{
    public static class ConfigureRFUserEmailDapper
    {
        public static void Setup(IDbConnection connection)
        {
            (new UserEmailDapper(connection)).CreateTable();
        }
    }
}