using RFAuth.Entities;
using RFAuth.IRepo;
using RFDapper;
using System.Data;

namespace RFAuthDapper
{
    public class UsersTypes(IDbConnection Connection) : Dapper<UserType>(Connection), IUserTypeRepo { }
    public class Users(IDbConnection Connection) : Dapper<User>(Connection), IUserRepo { }
    public class Passwords(IDbConnection Connection) : Dapper<Password>(Connection), IPasswordRepo { }
    public class Devices(IDbConnection Connection) : Dapper<Device>(Connection), IDeviceRepo { }
    public class Sessions(IDbConnection Connection) : Dapper<Session>(Connection), ISessionRepo { }
}
