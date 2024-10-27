using RFService.EntitiesLib;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("UsersTypes", Schema = "auth")]
    public class UserType : EntityTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}