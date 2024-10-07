using RFDapper.DapperLib;
using RFUserEmail.Entities;
using RFUserEmail.IRepo;
using System.Data;

namespace RFUserEmailDapper.Dapper
{
    public class UserEmailDapper(IDbConnection connection) : DapperBase<UserEmail>(connection), IUserEmailRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "UserEmail";
    }
}
