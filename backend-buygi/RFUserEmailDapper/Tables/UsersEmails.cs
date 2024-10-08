using RFDapper.DapperLib;
using RFService.ServicesLib;
using RFUserEmail.Entities;
using RFUserEmail.IRepo;
using System.Data;

namespace RFUserEmailDapper.Tables
{
    [Foreign("UserId", "Users", "Id", ForeignSchema: "auth")]
    public class UsersEmails(IDbConnection connection) : DapperBase<UserEmail>(connection), IUserEmailRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "UsersEmails";
    }
}
