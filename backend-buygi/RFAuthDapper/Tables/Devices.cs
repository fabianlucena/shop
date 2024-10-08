using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper.DapperLib;
using System.Data;

namespace RFAuthDapper.Tables
{
    public class Devices(IDbConnection connection) : DapperBase<Device>(connection), IDeviceRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Devices";
    }
}
