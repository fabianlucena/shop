using backend_buygi.Entities;
using backend_buygi.Repo;
using System.Data;

namespace backend_buygi.Dapper
{
    public class SessionDapper : DapperBase<Session>, ISessionRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Sessions";

        public SessionDapper(IDbConnection connection)
            : base(connection) { }
    }
}
