using Dapper;
using RFService.RepoLib;
using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace RFDapper.DapperLib
{
    public abstract class DapperBase<Entity>(IDbConnection Connection)
        where Entity : class
    {
        protected virtual string Schema { get; } = "dbo";
        protected abstract string TableName { get; }

        public void CreateTable()
        {
            var query = $@"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE [name] = '{Schema}')
                EXEC('CREATE SCHEMA [{Schema}] AUTHORIZATION [dbo]');";
            Connection.Query(query);

            var properties = typeof(Entity).GetProperties();
            var columnsQueries = new List<string>();
            var postQueries = new List<string>();
            foreach (var p in properties)
            {
                var columnQuery = p.Name;

                var propertyType = p.PropertyType;
                if (propertyType == typeof(long))
                    columnQuery += " BIGINT";
                else if (propertyType == typeof(string))
                    columnQuery += $" NVARCHAR({p.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 255})";
                else if (propertyType == typeof(Guid))
                    columnQuery += " UNIQUEIDENTIFIER";
                else if (propertyType == typeof(DateTime))
                    columnQuery += " DATETIME";
                else if (propertyType == typeof(DateTime?))
                    columnQuery += " DATETIME";
                else if (propertyType == typeof(bool))
                    columnQuery += " BIT";
                else
                    throw new Exception($"Unknown type {propertyType.Name}");

                if (p.GetCustomAttribute<AutoincrementAttribute>() != null)
                {
                    columnQuery += " IDENTITY(1,1)";
                    postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_PK] PRIMARY KEY NONCLUSTERED({p.Name})");
                }

                if (p.CustomAttributes.Any(a => a.AttributeType.Name == "RequiredMemberAttribute"))
                    columnQuery += " NOT NULL";
                else if (Nullable.GetUnderlyingType(propertyType) != null)
                    columnQuery += " NULL";
                else
                    columnQuery += " NOT NULL";

                if (p.GetCustomAttribute<UniqueAttribute>() != null)
                    postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_UK_{p.Name}] UNIQUE ([{p.Name}])");

                columnsQueries.Add(columnQuery);
            }

            var foreigns = this.GetType().GetCustomAttributes<ForeignAttribute>();
            foreach (var foreign in foreigns)
            {
                postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_{foreign.Column}_FK_{foreign.ForeignSchema}_{foreign.ForeignTable}_{foreign.ForeignColumn}]"
                    + $" FOREIGN KEY([{foreign.Column}]) REFERENCES [{foreign.ForeignSchema}].[{foreign.ForeignTable}]([{foreign.ForeignColumn}])"
                );
            }

            var columnsQuery = string.Join(",\r\n\t\t", [.. columnsQueries]);
            if (postQueries.Count > 0)
                columnsQuery += ",\r\n\t\t" + string.Join(",\r\n\t\t", [.. postQueries]);

            query = $"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'{Schema}.{TableName}') AND type in (N'U'))"
	            + $"\r\n\tCREATE TABLE [{Schema}].[{TableName}] (\r\n\t\t{columnsQuery}\r\n\t) ON [PRIMARY]";

            Connection.Query(query);
        }

        public string GetSelectQuery(GetOptions? options)
        {
            var sql = $"SELECT * FROM [{Schema}].[{TableName}]";
            if (options != null)
            {
                if (options.Filters != null)
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
            pId?.SetValue(data, id);
        }

        public async Task<Entity> InsertAsync(Entity data)
        {
            var query = GetInsertQuery(data);
            var rows = await Connection.QueryAsync<long>(query, data);
            long id = rows.First();
            DapperBase<Entity>.SetId(data, id);
            return data;
        }

        public Task<Entity?> GetSingleOrNullAsync(GetOptions? options)
        {
            return Connection.QuerySingleOrDefaultAsync<Entity>(GetSelectQuery(options), options?.Filters);
        }

        public Task<Entity> GetSingleAsync(GetOptions? options)
        {
            return Connection.QuerySingleAsync<Entity>(GetSelectQuery(options), options?.Filters);
        }

        public Task<IEnumerable<Entity>> GetListAsync(GetOptions? options)
        {
            return Connection.QueryAsync<Entity>(GetSelectQuery(options), options?.Filters);
        }
    }
}
