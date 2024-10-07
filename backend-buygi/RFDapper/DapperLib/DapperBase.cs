using Dapper;
using RFService.RepoLib;
using System.Data;

/*
 * 
        private readonly IDbConnection _dbConnection;

        public LocalPasswordDapper(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public LocalPassword? GetSingleFor(GetOptions? options)
        {
            var sql = "SELECT * FROM acc.LocalPasswords";
            return _dbConnection.Query<LocalPassword>(sql).ToList();
        }*/

namespace RFDapper.DapperLib
{
    public abstract class DapperBase<Entity>
        where Entity : class
    {
        protected virtual IDbConnection Connection { get; set; }
        protected virtual string Schema { get; } = "dbo";
        protected abstract string TableName { get; }

        public DapperBase(IDbConnection connection)
        {
            Connection = connection;
        }

        public string GetSelectQuery(GetOptions? options)
        {
            var sql = $"SELECT * FROM [{Schema}].[{TableName}]";
            if (options != null && options.Filters != null)
            {
                var properties = options.Filters.GetType().GetProperties();
                string separator = "  WHERE ";
                foreach (var p in properties)
                {
                    string name = p.Name;
                    sql += $"{separator}{name} = @{name}";
                    separator = " AND ";
                }
            }

            return sql;
        }

        public string GetInsertQuery(Entity data)
        {
            var properties = data.GetType().GetProperties();
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var p in properties)
            {
                string name = p.Name;
                if (name == "Id")
                {
                    continue;
                }

                columns.Add(name);
                values.Add("@" + name);
            }

            var sql = $"INSERT INTO [{Schema}].[{TableName}]({string.Join(",", columns)}) VALUES ({string.Join(",", values)}); SELECT CAST(SCOPE_IDENTITY() as INT);";
            return sql;
        }

        static void SetId(Entity data, long id)
        {
            var type = data.GetType();
            var pId = type.GetProperty("Id");
            if (pId != null)
            {
                pId.SetValue(data, id);
            }
        }

        public async Task<Entity> Insert(Entity data)
        {
            var query = GetInsertQuery(data);
            var rows = await Connection.QueryAsync<long>(query, data);
            long id = rows.First();
            DapperBase<Entity>.SetId(data, id);
            return data;
        }

        public Task<Entity?> GetSingleOrNull(GetOptions? options)
        {
            return Connection.QuerySingleOrDefaultAsync<Entity>(GetSelectQuery(options), options?.Filters);
        }

        public Task<Entity> GetSingle(GetOptions? options)
        {
            return Connection.QuerySingleAsync<Entity>(GetSelectQuery(options), options?.Filters);
        }

        public Task<IEnumerable<Entity>> GetList(GetOptions? options)
        {
            return Connection.QueryAsync<Entity>(GetSelectQuery(options), options?.Filters);
        }
    }
}
