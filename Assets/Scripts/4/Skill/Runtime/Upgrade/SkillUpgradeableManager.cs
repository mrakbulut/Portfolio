using Portfolio.Upgradeables;
using Portfolio.Utility;
using UnityEngine;

namespace Portfolio.Skill
{
    public class SkillUpgradeableManager : MonoBehaviour
    {
        [SerializeField] private SkillData[] _upgradeableSkillDatas;

        private SkillManager _skillManager;
        private IUpgradeable[] _upgradeableSkills;

        private void Start()
        {
            _skillManager = ServiceLocator.Instance.Get<SkillManager>();
            var upgradeableManager = ServiceLocator.Instance.Get<UpgradeableManager>();

            _upgradeableSkills = new IUpgradeable[_upgradeableSkillDatas.Length];
            for (int i = 0; i < _upgradeableSkillDatas.Length; i++)
            {
                _upgradeableSkills[i] = new SkillUpgradeable(_upgradeableSkillDatas[i]);
                _upgradeableSkills[i].OnUpgraded += OnSkillUpgraded;
                _upgradeableSkills[i].OnActivated += OnSkillActivated;
                upgradeableManager.AddUpgrade(_upgradeableSkills[i]);
            }
        }
        private void OnSkillActivated(IUpgradeable upgradeable)
        {
            if (upgradeable is SkillUpgradeable skillUpgrade)
            {
                Debug.Log("SKILL ACTIVATING : " + skillUpgrade.SkillData.SkillName, skillUpgrade.SkillData);
                _skillManager.AddSkill(skillUpgrade.SkillData);
            }
        }
        private void OnSkillUpgraded(IUpgradeable upgradeable)
        {
            if (upgradeable is SkillUpgradeable skillUpgrade)
            {
                _skillManager.LevelUpSkill(skillUpgrade.SkillData.Id);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _upgradeableSkills.Length; i++)
            {
                _upgradeableSkills[i].OnUpgraded -= OnSkillUpgraded;
                _upgradeableSkills[i].OnActivated -= OnSkillActivated;
            }
        }
    }
}
