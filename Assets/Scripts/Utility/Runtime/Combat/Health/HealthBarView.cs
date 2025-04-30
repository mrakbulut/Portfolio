using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.Utility
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _dynamicFillImage;
        [SerializeField] private float _fillDuration = 1f;

        private Health _health;
        private Vector3 _cameraPosition;

        private bool _showing;

        public void Initialize(Health health)
        {
            _health = health;
            _health.OnHealthChanged += HandleHealthChange;

            HandleHealthChange(_health.CurrentHealth, _health.MaxHealth);
        }

        private void HandleHealthChange(float currentHealth, float maxHealth)
        {
            float fillAmount = currentHealth / maxHealth;
            _fillImage.fillAmount = fillAmount;

            _dynamicFillImage.FillAmountAnimation(fillAmount, _fillDuration, Ease.Linear);

            bool dead = currentHealth == 0;
            gameObject.SetActive(_showing && !dead);
        }

        public void Show()
        {
            _showing = true;
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            _showing = false;
            gameObject.SetActive(false);
        }
    }
}
