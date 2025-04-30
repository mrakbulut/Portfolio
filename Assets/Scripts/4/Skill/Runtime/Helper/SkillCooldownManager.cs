using System;
using Portfolio.Stats;

namespace Portfolio.Skill
{
    public class SkillCooldownManager
    {
        private readonly Stat _cooldownStat;
        private float _currentTotalCooldown;
        private float _remainingCooldown;
        private bool _isOnCooldown;

        public bool IsOnCooldown => _isOnCooldown;
        public float RemainingCooldown => _remainingCooldown;
        public float CooldownPercentage => _isOnCooldown ? _remainingCooldown / _currentTotalCooldown : 0f;

        public Action OnCooldownStarted = () => { };
        public Action OnCooldownFinished = () => { };
        public Action<float> OnCooldownUpdated = _ => { };

        public SkillCooldownManager(Stat cooldownStat)
        {
            _cooldownStat = cooldownStat;
            _isOnCooldown = false;
            _remainingCooldown = 0f;

            _currentTotalCooldown = cooldownStat.TotalValue;
            _cooldownStat.OnTotalValueChanged += OnCooldownStatChanged;
        }

        ~SkillCooldownManager()
        {
            if (_cooldownStat != null)
            {
                _cooldownStat.OnTotalValueChanged -= OnCooldownStatChanged;
            }
        }

        private void OnCooldownStatChanged(float newValue)
        {
            if (!_isOnCooldown) return;

            float previousTotal = _currentTotalCooldown;
            float ratio = newValue / previousTotal;

            _remainingCooldown *= ratio;
            _currentTotalCooldown = _cooldownStat.TotalValue;
            OnCooldownUpdated?.Invoke(CooldownPercentage);
        }

        public bool Update(float deltaTime)
        {
            if (!_isOnCooldown) return false;

            _remainingCooldown -= deltaTime;

            // Notify about progress
            OnCooldownUpdated?.Invoke(CooldownPercentage);

            if (_remainingCooldown <= 0f)
            {
                _remainingCooldown = 0f;
                _isOnCooldown = false;

                OnCooldownFinished?.Invoke();
                return true;
            }

            return false;
        }

        public void StartCooldown()
        {
            _remainingCooldown = _currentTotalCooldown;
            _isOnCooldown = true;

            OnCooldownStarted?.Invoke();
            OnCooldownUpdated?.Invoke(1f); // Start at full cooldown (100%)
        }

        public void FinishCooldown()
        {
            if (!_isOnCooldown) return;

            _remainingCooldown = 0f;
            _isOnCooldown = false;

            OnCooldownFinished?.Invoke();
            OnCooldownUpdated?.Invoke(0f);
        }
    }
}
