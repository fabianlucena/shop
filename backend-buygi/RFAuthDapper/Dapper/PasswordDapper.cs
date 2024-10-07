using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper.DapperLib;
using System.Data;

namespace RFAuthDapper.Dapper
{
    public class PasswordDapper(IDbConnection connection) : DapperBase<Password>(connection), IPasswordRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Passwords";
    }
}
