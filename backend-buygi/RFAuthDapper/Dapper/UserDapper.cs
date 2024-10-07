using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper.DapperLib;
using System.Data;

namespace RFAuthDapper.Dapper
{
    public class UserDapper(IDbConnection connection) : DapperBase<User>(connection), IUserRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Users";
    }
}
