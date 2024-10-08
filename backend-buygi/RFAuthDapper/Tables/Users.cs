using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper.DapperLib;
using System.Data;

namespace RFAuthDapper.Tables
{
    public class Users(IDbConnection connection) : DapperBase<User>(connection), IUserRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Users";
    }
}
