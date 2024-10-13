using RFDapper;
using RFUserEmail.Entities;
using RFUserEmail.IRepo;
using System.Data;

namespace RFUserEmailDapper
{
    public class UsersEmails(IDbConnection Connection) : Dapper<UserEmail>(Connection), IUserEmailRepo { }
}
