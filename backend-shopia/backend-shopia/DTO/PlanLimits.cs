using backend_shopia.Entities;
using backend_shopia.Exceptions;

namespace backend_shopia.DTO
{
    public class PlanLimits
    {
        private List<PlanLimit> Limits { get; } = [];

        public Int64 this[PlanLimitName name]
        {
            get => Limits.Find(l => l.Name == name.ToString())?.Limit
                ?? throw new NoLimitNameInPlanException(name.ToString());
        }

        public PlanLimits(IEnumerable<PlanLimit> limits)
        {
            Limits.AddRange(limits);
        }

        public Dictionary<string, Int64> ToDictionary()
            => Limits.ToDictionary(l => l.Name, l => l.Limit);

        public Dictionary<string, Int64> ToDictionaryLCFirst()
            => Limits.ToDictionary(l => char.ToLowerInvariant(l.Name[0]) + l.Name[1..],l => l.Limit);
    }
}
