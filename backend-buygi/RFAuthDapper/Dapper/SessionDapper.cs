using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper.DapperLib;
using System.Data;

namespace RFAuthDapper.Dapper
{
    public class SessionDapper(IDbConnection connection) : DapperBase<Session>(connection), ISessionRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Sessions";
    }
}
