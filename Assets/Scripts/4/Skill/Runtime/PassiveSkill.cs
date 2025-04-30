using Portfolio.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Skill
{
    public class PassiveSkill : SkillBase
    {
        [TitleGroup("Passive Aura Prop")]
        [HorizontalGroup("Passive Aura Prop/Stats")]
        [SerializeField] private StatType[] _affectedStats;

        protected override void OnInitialized()
        {
            Idle();
            Activate();
        }

        private void Idle()
        {
            var idle = new IdleState(this);
            stateMachine.SwitchState(idle);
        }

        public override void Activate()
        {
            if (Active) return;

            ApplyPassiveEffects();
            Active = true;
            IsOnCooldown = false;
            Executing = true;
            OnActiveStatusChanged.Invoke(Active);
        }
        public override void Deactivate()
        {
            if (!Active) return;

            RemovePassiveEffects();
            Active = false;
            IsOnCooldown = false;
            Executing = false;
            OnActiveStatusChanged.Invoke(Active);
        }

        protected override void OnLevelChanged()
        {
            RemovePassiveEffects();
            ApplyPassiveEffects();
        }

        private void ApplyPassiveEffects()
        {
            // Apply skill effects to owner's stats
            foreach (var affectedStat in _affectedStats)
            {
                var statTypeId = affectedStat.Id;

                var skillStat = statContainer.GetStatByStatTypeId(statTypeId);
                var ownerStat = ownerStatContainer.GetStatByStatTypeId(statTypeId);

                if (ownerStat != null)
                {
                    float modifierValue = skillStat.TotalValue;
                    var modifier = new StatModifier(modifierValue, StatModifierType.Flat, this);
                    ownerStat.AddModifier(modifier);

                    Debug.Log($"Applied passive bonus to {affectedStat.name}: {modifierValue}");
                }
            }
        }

        private void RemovePassiveEffects()
        {
            // Remove all modifiers from owner stats that were added by this skill
            ownerStatContainer.RemoveModifiersFromSource(this);
        }

        private void OnDisable()
        {
            if (Active)
            {
                Deactivate();
            }
        }

        private void OnDestroy()
        {
            if (Active)
            {
                Deactivate();
            }
        }
    }
}
