using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Skill
{
    public class SkillUIManager : MonoBehaviour
    {
        [SerializeField] private SkillUIFactory _skillUIFactory;

        private readonly Dictionary<SkillBase, ISkillUI> _skillUIs = new Dictionary<SkillBase, ISkillUI>();

        public void OnSkillAdd(SkillBase skill, SkillData skillData)
        {
            var skillUI = _skillUIFactory.GetSkillUI(skill, skillData);
            _skillUIs.Add(skill, skillUI);
        }

        public void RemoveAllSkillUIs()
        {
            foreach (var skillUI in _skillUIs.Values)
            {
                skillUI.Terminate();
            }

            _skillUIs.Clear();
        }

        public void RemoveSkillUI(SkillBase skill)
        {
            if (!_skillUIs.TryGetValue(skill, out var skillUI)) return;

            skillUI.Terminate();
            _skillUIs.Remove(skill);
        }

        public void ShowAllSkills()
        {
            foreach (var skillUI in _skillUIs.Values)
            {
                skillUI.Show();
            }
        }

        public void HideAllSkills()
        {
            foreach (var skillUI in _skillUIs.Values)
            {
                skillUI.Hide();
            }
        }
    }
}
