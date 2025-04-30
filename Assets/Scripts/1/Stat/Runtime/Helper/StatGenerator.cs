using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Stats
{
    public class StatGenerator
    {
        public List<Stat> GenerateStats(BaseStatContainer baseStatContainer)
        {
            if (baseStatContainer is null)
            {
                Debug.LogError("Dice Face Base Stat Container is null");
                return new List<Stat>();
            }

            var levelBaseStat = baseStatContainer.GetBaseStatsByLevel(0);
            if (levelBaseStat is null)
            {
                Debug.LogError("Dice Face Base Stat Container is null");
                return new List<Stat>();
            }

            var stats = new List<Stat>();

            foreach (var baseStat in levelBaseStat.BaseStats)
            {
                var stat = GenerateStat(baseStat);
                stats.Add(stat);
            }

            return stats;
        }

        private Stat GenerateStat(BaseStat baseStat)
        {
            if (baseStat is null)
            {
                Debug.LogError("Dice Face Base Stat is null");
                return null;
            }

            var stat = new Stat(baseStat.Value, baseStat.StatType.Id);
            return stat;
        }
    }
}
