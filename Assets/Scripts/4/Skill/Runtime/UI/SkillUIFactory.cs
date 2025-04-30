using UnityEngine;

namespace Portfolio.Skill
{
    public class SkillUIFactory : MonoBehaviour
    {
        [SerializeField] private Transform _skillUIContainer;

        [SerializeField] private GameObject _passiveSkillUIPrefab;
        [SerializeField] private GameObject _passiveAuraSkillUIPrefab;
        [SerializeField] private GameObject _activeSkillUIPrefab;

        public ISkillUI GetSkillUI(SkillBase skill, SkillData skillData)
        {
            if (skill is PassiveSkill passiveSkill)
            {
                var passiveSkillGO = Instantiate(_passiveSkillUIPrefab, _skillUIContainer);

                var passiveSkillUI = passiveSkillGO.GetComponent<PassiveSkillUI>();
                passiveSkillUI.Initialize(passiveSkill, skillData);

                return passiveSkillUI;
            }
            if (skill is PassiveAuraSkill passiveAuraSkill)
            {
                var passiveSkillGO = Instantiate(_passiveAuraSkillUIPrefab, _skillUIContainer);

                var passiveAuraSkillUI = passiveSkillGO.GetComponent<PassiveAuraSkillUI>();
                passiveAuraSkillUI.Initialize(passiveAuraSkill, skillData);

                return passiveAuraSkillUI;
            }
            if (skill is ActiveSkill activeSkill)
            {
                var activeSkillGO = Instantiate(_activeSkillUIPrefab, _skillUIContainer);

                var activeSkillUI = activeSkillGO.GetComponent<ActiveSkillUI>();
                activeSkillUI.Initialize(activeSkill, skillData);

                return activeSkillUI;
            }


            Debug.LogError("Skill UI factory could not be instantiated." + skill, skill);
            return null;
        }
    }
}
