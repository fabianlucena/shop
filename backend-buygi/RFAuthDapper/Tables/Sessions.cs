using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper.DapperLib;
using RFService.ServicesLib;
using System.Data;

namespace RFAuthDapper.Tables
{
    [Foreign("UserId", "Users", "Id", ForeignSchema: "auth")]
    [Foreign("DeviceId", "Devices", "Id", ForeignSchema: "auth")]
    public class Sessions(IDbConnection connection) : DapperBase<Session>(connection), ISessionRepo
    {
        protected override string Schema { get; } = "auth";
        protected override string TableName { get; } = "Sessions";
    }
}
