using Portfolio.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Skill
{
    public class PassiveAuraSkill : SkillBase
    {
        [TitleGroup("Passive Aura Prop")]
        [TitleGroup("Passive Aura Prop/Detection")]
        [SerializeField] private LayerMask _affectedLayers;

        [PropertySpace(float.Epsilon)]
        [TitleGroup("Passive Aura Prop/Stats")]
        [SerializeField] private StatType[] _affectedStats;
        [TitleGroup("Passive Aura Prop/Stats")]
        [SerializeField] private StatType _areaStatType;
        [TitleGroup("Passive Aura Prop/Stats")]
        [SerializeField] private StatType _cooldownStatType;

        [PropertySpace(float.Epsilon)]
        [TitleGroup("Passive Aura Prop/Settings")]
        [SerializeField] private bool _affectOwner;

        private Stat _areaStat;
        private Stat _cooldownStat;

        private UnitDetectorInArea _effectUnitDetectorInArea;
        private AreaEffectUnitManager _effectUnitManager;

        protected override void OnInitialized()
        {
            _effectUnitDetectorInArea = new UnitDetectorInArea(
                transform,
                _affectedLayers,
                owner,
                _affectOwner
                );

            _effectUnitManager = new AreaEffectUnitManager(statContainer, this);

            Idle();
            SetStats();
            Activate();
        }

        private void SetStats()
        {
            _areaStat = statContainer.GetStatByStatTypeId(_areaStatType.Id);
            _cooldownStat = statContainer.GetStatByStatTypeId(_cooldownStatType.Id);
        }

        private void Idle()
        {
            var idle = new IdleState(this);
            stateMachine.SwitchState(idle);
        }

        private void Pulse()
        {
            var pulseStateData = new PassiveAuraSkillPulseStateData
            {
                AffectedStats = _affectedStats,
                AreaStat = _areaStat,
                CooldownStat = _cooldownStat
            };

            var pulse = new PassiveAuraSkillPulseState(this, pulseStateData, _effectUnitDetectorInArea, _effectUnitManager);
            stateMachine.SwitchState(pulse);
        }

        protected override void OnLevelChanged()
        {
            Idle();
            Pulse();
        }

        public override void Activate()
        {
            if (Active) return;

            Active = true;
            IsOnCooldown = false;
            Executing = true;
            OnActiveStatusChanged.Invoke(Active);
            Pulse();
        }

        public override void Deactivate()
        {
            if (!Active) return;

            Idle();
            _effectUnitManager.RemoveAllEffects();
            Active = false;
            IsOnCooldown = false;
            Executing = false;
            OnActiveStatusChanged.Invoke(Active);
        }
    }

}
