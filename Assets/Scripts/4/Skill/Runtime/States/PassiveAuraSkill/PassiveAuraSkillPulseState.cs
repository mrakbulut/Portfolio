using System.Collections.Generic;
using Portfolio.StateMachines;
using Portfolio.Stats;
using Portfolio.Utility;
using UnityEngine;

namespace Portfolio.Skill
{
    public class PassiveAuraSkillPulseState : IState
    {
        private readonly PassiveAuraSkill _passiveAuraSkill;
        private readonly List<SerializableGuid> _affectedStatTypeIds = new List<SerializableGuid>();
        private readonly Stat _areaStat;
        private readonly Stat _cooldownStat;

        private readonly UnitDetectorInArea _effectUnitDetectorInArea;
        private readonly AreaEffectUnitManager _effectUnitManager;

        private readonly HashSet<GameObject> _previouslyAffected = new HashSet<GameObject>();
        private readonly HashSet<GameObject> _currentlyAffected = new HashSet<GameObject>();

        private float _timer;
        private float _cooldown;
        private float _area;

        public PassiveAuraSkillPulseState(PassiveAuraSkill passiveAuraSkill, PassiveAuraSkillPulseStateData stateData, UnitDetectorInArea effectUnitDetectorInArea, AreaEffectUnitManager effectUnitManager)
        {
            _passiveAuraSkill = passiveAuraSkill;
            _areaStat = stateData.AreaStat;
            _cooldownStat = stateData.CooldownStat;

            foreach (var affectedStatType in stateData.AffectedStats)
            {
                _affectedStatTypeIds.Add(affectedStatType.Id);
            }

            _effectUnitDetectorInArea = effectUnitDetectorInArea;
            _effectUnitManager = effectUnitManager;
        }

        public void Enter()
        {
            ListenStats();

            _area = _areaStat.TotalValue;
            _cooldown = _cooldownStat.TotalValue;

            _timer = 0f;

            PulseAura();
        }

        public void Exit()
        {
            StopListeningStats();
            _effectUnitManager.RemoveAllEffects();

            _previouslyAffected.Clear();
            _currentlyAffected.Clear();
        }

        public void Tick(float deltaTime)
        {
            _timer -= deltaTime;

            if (_timer <= 0f)
            {
                _timer = _cooldown;
                PulseAura();
            }
        }

        private void ListenStats()
        {
            _areaStat.OnTotalValueChanged += OnAreaValueChanged;
            _cooldownStat.OnTotalValueChanged += OnCooldownValueChanged;
        }

        private void StopListeningStats()
        {
            _areaStat.OnTotalValueChanged -= OnAreaValueChanged;
            _cooldownStat.OnTotalValueChanged -= OnCooldownValueChanged;
        }

        private void OnAreaValueChanged(float area)
        {
            _area = area;
        }

        private void OnCooldownValueChanged(float cooldown)
        {
            _cooldown = cooldown;
        }

        private void PulseAura()
        {
            // Store previously affected units
            _previouslyAffected.Clear();
            foreach (var unit in _effectUnitManager.GetAllAffectedTargets())
            {
                _previouslyAffected.Add(unit);
            }

            // Find currently affected units
            _currentlyAffected.Clear();
            var detectedUnits = _effectUnitDetectorInArea.DetectUnitsInArea(_area);
            foreach (var unit in detectedUnits)
            {
                _currentlyAffected.Add(unit);
            }

            // Apply effect to new units
            foreach (var unit in _currentlyAffected)
            {
                if (!_previouslyAffected.Contains(unit))
                {
                    _effectUnitManager.ApplyEffect(unit, _affectedStatTypeIds);
                }
            }

            // Remove effect from units no longer in range
            foreach (var unit in _previouslyAffected)
            {
                if (!_currentlyAffected.Contains(unit))
                {
                    _effectUnitManager.RemoveEffect(unit);
                }
            }
        }
    }

    public struct PassiveAuraSkillPulseStateData
    {
        public StatType[] AffectedStats;
        public Stat AreaStat;
        public Stat CooldownStat;
    }
}
