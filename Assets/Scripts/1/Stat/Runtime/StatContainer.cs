using System.Collections.Generic;
using System.Linq;
using Portfolio.Utility;

namespace Portfolio.Stats
{
    public class StatContainer : IStatContainer
    {
        private readonly Dictionary<SerializableGuid, Stat> _stats;

        public StatContainer(BaseStatContainer baseStatContainer)
        {
            _stats = new Dictionary<SerializableGuid, Stat>();

            GenerateStats(baseStatContainer);
        }

        private void GenerateStats(BaseStatContainer baseStatContainer)
        {
            var statGenerator = new StatGenerator();

            foreach (var generatedStat in statGenerator.GenerateStats(baseStatContainer))
            {
                _stats.TryAdd(generatedStat.StatTypeId, generatedStat);
            }
        }

        public Stat GetStatByStatType(StatType statType)
        {
            return GetStatByStatTypeId(statType.Id);
        }

        public Stat GetStatByStatTypeId(SerializableGuid statTypeId)
        {
            if (_stats != null && _stats.TryGetValue(statTypeId, out var stat)) return stat;

            return null;
        }

        public void RemoveModifiersFromSource(object source)
        {
            foreach (var stat in _stats)
            {
                stat.Value.RemoveSourceModifiers(source);
            }
        }

        public Stat[] GetStats()
        {
            return _stats.Values.ToArray();
        }
    }
}
