using System.Data;

namespace RFUserEmailDapper
{
    public static class Setup
    {
        public static void ConfigureRFUserEmailDapper(IDbConnection connection)
        {
            (new UsersEmails(connection)).CreateTable();
        }
    }
}