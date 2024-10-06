using backend_buygi.Entities;
using backend_buygi.Repo;
using System.Data;

namespace backend_buygi.Dapper
{
    public class UserDapper : DapperBase<User>, IUserRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Users";

        public UserDapper(IDbConnection connection)
            : base(connection) { }
    }
}
