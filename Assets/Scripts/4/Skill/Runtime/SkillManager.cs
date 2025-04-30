using System.Collections.Generic;
using Portfolio.Stats;
using Portfolio.Unit;
using Portfolio.Utility;
using UnityEngine;

namespace Portfolio.Skill
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private SkillUIManager _skillUIManager;

        private IUnit _ownerUnit;
        private GameObject _owner;
        private IStatContainer _ownerStatContainer;

        private readonly Dictionary<SerializableGuid, SkillBase> _activeSkills = new Dictionary<SerializableGuid, SkillBase>();

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
        }

        public void Initialize(IUnit ownerUnit, GameObject owner, IStatContainer ownerStatContainer)
        {
            _owner = owner;
            _ownerUnit = ownerUnit;
            _ownerStatContainer = ownerStatContainer;
        }

        public SkillBase AddSkill(SkillData skillData)
        {
            var skillId = skillData.Id;
            if (_activeSkills.ContainsKey(skillId))
            {
                //Debug.Log("ADDING SKILL_-1 : " + skillData.SkillName);
                Debug.LogWarning($"Skill {skillData.SkillName} already exists in the active skills");
                return _activeSkills[skillId];
            }

            var skillObject = Instantiate(skillData.Prefab, transform);
            var skillBase = skillObject.GetComponent<SkillBase>();
            skillBase.Initialize(_ownerUnit, _owner, _ownerStatContainer);
            skillBase.Activate();

            _skillUIManager.OnSkillAdd(skillBase, skillData);

            _activeSkills.Add(skillId, skillBase);
            return skillBase;
        }

        public void RemoveSkill(SerializableGuid skillId)
        {
            if (_activeSkills.TryGetValue(skillId, out var skill))
            {
                skill.Deactivate();
                Destroy(skill.gameObject);
                _activeSkills.Remove(skillId);
                _skillUIManager.RemoveSkillUI(skill);
            }
        }

        public SkillBase GetSkill(SerializableGuid skillId)
        {
            if (_activeSkills.TryGetValue(skillId, out var skill))
            {
                return skill;
            }

            return null;
        }

        public void LevelUpSkill(SerializableGuid skillId)
        {
            if (_activeSkills.TryGetValue(skillId, out var skill))
            {
                //Debug.Log("UPGRADING SKILL : " + skill.gameObject, skill.gameObject);
                skill.LevelUp();
            }
            else
            {
                Debug.LogWarning($"Tried to level up nonexistent skill: {skillId}");
            }
        }
    }
}
