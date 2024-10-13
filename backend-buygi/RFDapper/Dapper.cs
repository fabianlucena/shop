using Dapper;
using RFService.IRepo;
using RFService.RepoLib;
using RFService.ServicesLib;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace RFDapper
{
    public class Dapper<Entity> : IRepo<Entity>
        where Entity : class
    {
        private readonly IDbConnection _connection;
        private readonly string _tableName;
        private readonly string _schema = "dbo";

        public IDbConnection Connection { get => _connection; }
        public string TableName { get => _tableName; }
        public string Schema { get => _schema; }

        public Dapper(IDbConnection Connection)
        {
            _connection = Connection;
            var tableAttribute = typeof(Entity).GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
            {
                _tableName = typeof(Entity).Name;
            }
            else
            {
                _tableName = tableAttribute.Name;
                if (!string.IsNullOrEmpty(tableAttribute.Schema))
                {
                    _schema = tableAttribute.Schema;
                }
            }
        }

        public void CreateTable()
        {
            var SQLTypes = new Dictionary<string, string>
                {
                    {"Int64", "BIGINT"},
                    {"Guid", "UNIQUEIDENTIFIER"},
                    {"DateTime", "DATETIME"},
                    {"Boolean", "BIT"},
                };

            var query = $@"IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE [name] = '{Schema}')
                EXEC('CREATE SCHEMA [{Schema}] AUTHORIZATION [dbo]');";
            Connection.Query(query);

            var entityType = typeof(Entity);
            var properties = entityType.GetProperties();
            var columnsQueries = new List<string>();
            var postQueries = new List<string>();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                bool? nullable = null;
                string propertyTypeName;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    nullable = true;
                    propertyTypeName = Nullable.GetUnderlyingType(propertyType)?.Name ?? "";
                } else
                {
                    propertyTypeName = propertyType.Name;
                }

                if (!SQLTypes.TryGetValue(propertyTypeName, out string? sqlType))
                {
                    if (!propertyType.IsClass)
                        throw new Exception($"Unknown type {propertyType.Name}");

                    if (propertyTypeName == "String")
                        sqlType = $"NVARCHAR({property.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 255})";
                }

                if (sqlType != null)
                {
                    var columnQuery = $"[{property.Name}] {sqlType}";

                    if (property.CustomAttributes.Any(a => a.AttributeType.Name == "RequiredMemberAttribute"))
                        columnQuery += " NOT NULL";
                    else if (Nullable.GetUnderlyingType(propertyType) != null)
                        columnQuery += " NULL";
                    else if (nullable != null)
                    {
                        columnQuery += (bool)nullable ?
                            " NULL" :
                            " NOT NULL";
                    }

                    var settedPk = false;
                    var databaseGeneratedAttribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (databaseGeneratedAttribute != null)
                    {
                        if (databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        {
                            columnQuery += " IDENTITY(1,1)";
                            postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_PK] PRIMARY KEY NONCLUSTERED ({property.Name})");
                            settedPk = true;
                        }
                    }

                    if (!settedPk && property.GetCustomAttribute<KeyAttribute>() != null)
                        postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_PK_{property.Name}] PRIMARY KEY CLUSTERED ([{property.Name}])");

                    columnsQueries.Add(columnQuery);
                }

                var foreign = property.GetCustomAttribute<ForeignKeyAttribute>();
                if (foreign != null)
                {
                    var foreignObject = entityType.GetProperty(foreign.Name) ??
                        throw new Exception($"Unknown foreign {foreign.Name}");

                    var foreignObjectType = foreignObject.PropertyType;
                    var referenceColumn = property;
                    if (!foreignObjectType.IsClass)
                    {
                        if (!propertyType.IsClass)
                            throw new Exception($"Foreign is not and object{foreign.Name}");

                        referenceColumn = foreignObject;
                        foreignObject = property;

                        foreignObjectType = propertyType;
                    }

                    var foreignTableAttribute = foreignObjectType.GetCustomAttribute<TableAttribute>();
                    string foreignTable = "",
                        foreignSchema = "dbo",
                        foreignColumn = "Id";
                    if (foreignTableAttribute == null)
                        foreignTable = typeof(Entity).Name;
                    else
                    {
                        foreignTable = foreignTableAttribute.Name;
                        if (!string.IsNullOrEmpty(foreignTableAttribute.Schema))
                            foreignSchema = foreignTableAttribute.Schema;
                    }

                    postQueries.Add($"CONSTRAINT [{Schema}_{TableName}_{referenceColumn.Name}_FK_{foreignSchema}_{foreignTable}_{foreignColumn}]"
                        + $" FOREIGN KEY([{referenceColumn.Name}]) REFERENCES [{foreignSchema}].[{foreignTable}]([{foreignColumn}])"
                    );
                }
            }

            var indexes = entityType.GetCustomAttributes<IndexAttribute>();
            if (indexes != null) {
                foreach (var index in indexes)
                {
                    var name = index.Name ?? $"{Schema}_{TableName}_{(index.IsUnique? "U": "I")}K_{string.Join('_', index.PropertyNames)}";
                    var indexType = index.IsUnique ?
                        "UNIQUE" :
                        "INDEX";

                    postQueries.Add($"CONSTRAINT [{name}] {indexType} ([{string.Join("], [", index.PropertyNames)}])");
                }
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
            var entityType = typeof(Entity);
            var properties = data.GetType().GetProperties();
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var p in properties)
            {
                string name = p.Name;
                if (name == "Id")
                    continue;

                var property = entityType.GetProperty(name) ??
                        throw new Exception($"Unknown {name} property");
                var propertyType = property.PropertyType;

                if (propertyType.IsClass && propertyType.Name != "String")
                    continue;

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
            SetId(data, id);
            return data;
        }

        public Task<Entity?> GetSingleOrDefaultAsync(GetOptions? options)
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
