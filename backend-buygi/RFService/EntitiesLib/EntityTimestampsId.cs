using RFService.ServicesLib;
using System.ComponentModel.DataAnnotations;

namespace RFService.EntitiesLib
{
    public abstract class EntityTimestampsId : EntityTimestamps
    {
        [Key]
        [Autoincrement]
        public Int64 Id { get; set; }
    }
}