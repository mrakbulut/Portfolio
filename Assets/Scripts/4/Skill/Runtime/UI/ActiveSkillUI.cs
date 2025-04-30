using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.Skill
{
    public class ActiveSkillUI : SkillUI
    {
        [TitleGroup("ActiveSkill")]
        [TitleGroup("ActiveSkill/Cooldown")]
        [SerializeField] private GameObject _cooldownView;
        [TitleGroup("ActiveSkill/Cooldown")]
        [SerializeField] private Image _cooldownFillImage;

        [TitleGroup("ActiveSkill/Execution")]
        [SerializeField] private GameObject _executionView;

        private SkillCooldownManager _skillCooldownManager;
        private ActiveSkill _activeSkill;

        public override void Initialize<T>(T activeSkill, SkillData skillData)
        {
            base.Initialize(activeSkill, skillData);

            _activeSkill = activeSkill as ActiveSkill;
            if (_activeSkill == null)
            {
                Debug.LogError("ActiveSkill is not ActiveSkill", gameObject);
                return;
            }

            _activeSkill.OnSkillExecutionStart += OnSkillExecutionStart;

            _skillCooldownManager = _activeSkill.CooldownManager;
            _skillCooldownManager.OnCooldownStarted += OnCooldownStarted;
            _skillCooldownManager.OnCooldownFinished += OnCooldownFinished;
            _skillCooldownManager.OnCooldownUpdated += OnCooldownUpdated;
        }

        private void OnSkillExecutionStart()
        {
            _executionView.SetActive(true);
            _cooldownView.SetActive(false);
        }

        private void OnCooldownUpdated(float cooldownRate)
        {
            _cooldownFillImage.fillAmount = cooldownRate;
        }
        private void OnCooldownFinished()
        {
            // TODO: Trigger Something ?
            _cooldownFillImage.fillAmount = 1f;
        }

        private void OnCooldownStarted()
        {
            _cooldownFillImage.fillAmount = 0f;

            _cooldownView.SetActive(true);
            _executionView.SetActive(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_activeSkill != null)
            {
                _activeSkill.OnSkillExecutionStart -= OnSkillExecutionStart;
            }
        }
    }
}
