using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper.DapperLib;
using RFService.ServicesLib;
using System.Data;

namespace RFAuthDapper.Tables
{
    [Foreign("UserId", "Users", "Id", ForeignSchema: "auth")]
    public class Passwords(IDbConnection connection) : DapperBase<Password>(connection), IPasswordRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Passwords";
    }
}
