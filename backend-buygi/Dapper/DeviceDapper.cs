using backend_buygi.Entities;
using backend_buygi.Repo;
using System.Data;

namespace backend_buygi.Dapper
{
    public class DeviceDapper : DapperBase<Device>, IDeviceRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Devices";

        public DeviceDapper(IDbConnection connection)
            :base(connection) { }
    }
}
