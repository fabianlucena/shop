using System.Text.Json.Serialization;

namespace RFService.EntitiesLib
{
    public abstract class EntityAttributes : Entity
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object>? Attributes { get; set; }
    }
}