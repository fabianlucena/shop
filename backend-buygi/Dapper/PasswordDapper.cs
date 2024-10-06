using backend_buygi.Entities;
using backend_buygi.Repo;
using System.Data;

namespace backend_buygi.Dapper
{
    public class PasswordDapper : DapperBase<Password>, IPasswordRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Passwords";

        public PasswordDapper(IDbConnection connection)
            : base(connection) { }
    }
}
