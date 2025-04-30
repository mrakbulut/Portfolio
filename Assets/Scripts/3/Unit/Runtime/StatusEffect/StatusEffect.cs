using System.Collections.Generic;
using Portfolio.Stats;

namespace Portfolio.Unit
{
    public abstract class StatusEffect
    {
        public readonly StatusEffectData Data;
        public readonly IUnit Source;
        public readonly int Level;
        public readonly float Duration;

        protected readonly Dictionary<IUnit, List<StatModifier>> AppliedModifiers = new Dictionary<IUnit, List<StatModifier>>();

        protected StatusEffect(StatusEffectData data, IUnit source, int level, float duration)
        {
            Data = data;
            Source = source;
            Level = level;
            Duration = duration;
        }

        public virtual void ApplyEffect(IUnit target)
        {
            var levelData = GetCurrentLevelData();
            foreach (var statTypeStatModifier in levelData.StatTypeStatModifiers)
            {
                var stat = target.StatContainer.GetStatByStatType(statTypeStatModifier.StatType);
                if (stat == null) continue;

                var modifier = new StatModifier(statTypeStatModifier.StatModifier, this);
                stat.AddModifier(modifier);
                AddModifier(target, modifier);
            }
        }
        public virtual void RemoveEffect(IUnit target)
        {
            target.StatContainer.RemoveModifiersFromSource(this);

            AppliedModifiers.Remove(target);
        }

        public virtual void UpdateEffect(IUnit target, float deltaTime) { }

        public StatusEffectData.StatusEffectLevelData GetCurrentLevelData()
        {
            return Data.GetLevelData(Level);
        }

        protected void AddModifier(IUnit target, StatModifier modifier)
        {
            if (!AppliedModifiers.TryGetValue(target, out var modifiers))
            {
                modifiers = new List<StatModifier>();
                AppliedModifiers[target] = modifiers;
            }
            modifiers.Add(modifier);
        }

        protected void RemoveModifier(IUnit target, StatModifier modifier)
        {
            if (AppliedModifiers.TryGetValue(target, out var modifiers))
            {
                modifiers.Remove(modifier);
                if (modifiers.Count == 0)
                {
                    AppliedModifiers.Remove(target);
                }
            }
        }

        public void RemoveEffectFromAllTargets()
        {
            foreach (var target in new List<IUnit>(AppliedModifiers.Keys))
            {
                RemoveEffect(target);
            }
            AppliedModifiers.Clear();
        }

        public abstract void Refresh();
    }
}
