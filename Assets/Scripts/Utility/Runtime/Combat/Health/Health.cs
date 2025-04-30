using UnityEngine;

namespace Portfolio.Utility
{
    public class Health
    {
        private float _currentHealth;
        private float _maxHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;

        public System.Action<float> OnDamageTake = _ => { };
        public System.Action<float> OnHeal = _ => { };
        public System.Action<float, float> OnHealthChanged = (_, _) => { };
        public System.Action OnDead = () => { };
        public System.Action OnRevive = () => { };

        public bool IsDead => _isDead;
        private bool _isDead;

        public Health(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = MaxHealth;
        }

        public void SetNewMaxHealth(float maxHealth)
        {
            float currentHealthPercentage = _currentHealth / MaxHealth;

            _maxHealth = maxHealth;

            float newHealth = MaxHealth * currentHealthPercentage;
            SetHealth(newHealth);
        }

        public void TakeDamage(float damage)
        {
            float newHealth = Mathf.Max(0, CurrentHealth - damage);
            SetHealth(newHealth);

            OnDamageTake.Invoke(CurrentHealth);

            if (CurrentHealth <= 0)
            {
                _isDead = true;
                OnDead.Invoke();
            }
        }

        public void Heal(float heal)
        {
            //Debug.Log("HEALING : " + heal);
            float newHealth = Mathf.Min(CurrentHealth + heal, MaxHealth);
            SetHealth(newHealth);

            OnHeal.Invoke(CurrentHealth);
        }

        public void Revive(float healthPercentage = 100f)
        {
            if (!_isDead) return;

            _isDead = false;
            float healAmount = MaxHealth * healthPercentage / 100f;
            Heal(healAmount);
            OnRevive.Invoke();
        }

        public void Reset()
        {
            _isDead = false;
            SetHealth(MaxHealth);
        }

        private void SetHealth(float newHealth)
        {
            _currentHealth = newHealth;

            OnHealthChanged.Invoke(CurrentHealth, MaxHealth);
        }
    }
}
