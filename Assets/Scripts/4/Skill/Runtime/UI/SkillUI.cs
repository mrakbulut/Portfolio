using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.Skill
{
    public abstract class SkillUI : MonoBehaviour, ISkillUI
    {
        [TitleGroup("References")]
        [SerializeField] protected GameObject root;
        [TitleGroup("References/ActiveStatus")]
        [SerializeField] protected GameObject activeGO;
        [TitleGroup("References/ActiveStatus")]
        [SerializeField] protected GameObject deActiveGO;
        [TitleGroup("References/UI")]
        [SerializeField] protected Image skillIcon;

        protected SkillBase skill;

        public virtual void Initialize<T>(T skillBase, SkillData skillData) where T : SkillBase
        {
            skill = skillBase;
            skill.OnActiveStatusChanged += OnActiveStatusChanged;

            skillIcon.sprite = skillData.Icon;
        }

        private void OnActiveStatusChanged(bool active)
        {
            activeGO.SetActive(active);
            deActiveGO.SetActive(!active);
        }

        protected virtual void OnDestroy()
        {
            if (skill == null) return;

            skill.OnActiveStatusChanged -= OnActiveStatusChanged;
        }

        public void Show()
        {
            root.SetActive(true);
        }
        public void Hide()
        {
            root.SetActive(false);
        }

        public virtual void Terminate()
        {
            Destroy(gameObject);
        }
    }
}
