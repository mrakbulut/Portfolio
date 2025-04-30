using System.Collections.Generic;
using Portfolio.Stats;
using Portfolio.Utility;
using UnityEngine;

namespace Portfolio.Skill
{
    public class AreaEffectUnitManager
    {
        private readonly IStatContainer _sourceStatContainer;
        private readonly object _modifierSource;

        private readonly List<GameObject> _appliedTargets = new List<GameObject>();

        public AreaEffectUnitManager(IStatContainer sourceStatContainer, object modifierSource)
        {
            _sourceStatContainer = sourceStatContainer;
            _modifierSource = modifierSource;
        }

        public void ApplyEffect(GameObject targetUnit, List<SerializableGuid> statTypeIds)
        {
            if (targetUnit == null) return;

            var unitStatContainer = targetUnit.GetComponent<IStatContainer>();
            if (unitStatContainer == null)
            {
                Debug.LogWarning($"Unit {targetUnit.name} does not have a stat container, skipping");
                return;
            }

            for (int i = 0; i < statTypeIds.Count; i++)
            {
                var statTypeId = statTypeIds[i];
                var sourceStat = _sourceStatContainer.GetStatByStatTypeId(statTypeId);
                var targetStat = unitStatContainer.GetStatByStatTypeId(statTypeId);

                if (sourceStat == null || targetStat == null) continue;

                var modifier = new StatModifier(sourceStat.TotalValue, StatModifierType.Flat, _modifierSource);
                targetStat.AddModifier(modifier);
            }

            _appliedTargets.Add(targetUnit);
        }

        public void RemoveEffect(GameObject targetUnit)
        {
            if (targetUnit == null) return;

            var unitStatContainer = targetUnit.GetComponent<IStatContainer>();
            if (unitStatContainer == null) return;

            unitStatContainer.RemoveModifiersFromSource(_modifierSource);

            _appliedTargets.Remove(targetUnit);
        }

        /// <summary>
        /// Remove all applied modifiers from all targets
        /// </summary>
        public void RemoveAllEffects()
        {
            foreach (var target in _appliedTargets)
            {
                var unitStatContainer = target.GetComponent<IStatContainer>();
                if (unitStatContainer == null) continue;

                unitStatContainer.RemoveModifiersFromSource(_modifierSource);
            }

            _appliedTargets.Clear();
        }

        /// <summary>
        /// Check if a target has active aura effects applied by this manager
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns>Whether the target has active effects</returns>
        public bool HasActiveEffects(GameObject target)
        {
            return _appliedTargets.Contains(target);
        }

        /// <summary>
        /// Get all targets that have active aura effects
        /// </summary>
        /// <returns>A collection of GameObjects with active effects</returns>
        public IEnumerable<GameObject> GetAllAffectedTargets()
        {
            return _appliedTargets;
        }
    }
}
