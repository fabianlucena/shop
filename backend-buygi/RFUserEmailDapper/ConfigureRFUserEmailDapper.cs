using RFUserEmailDapper.Tables;
using System.Data;

namespace RFUserEmailDapper
{
    public static class ConfigureRFUserEmailDapper
    {
        public static void Setup(IDbConnection connection)
        {
            (new UsersEmails(connection)).CreateTable();
        }
    }
}