using System.Collections.Generic;
using Portfolio.Utility;
using UnityEngine;

namespace Portfolio.Skill
{
    public class SkillBehaviourPoolManager : MonoBehaviour
    {
        [SerializeField] private List<SkillBehaviourPool> _skillBehaviourPools;

        private Dictionary<SerializableGuid, SkillBehaviourPool> _poolsByType;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            InitializePools();
        }
        private void InitializePools()
        {
            _poolsByType = new Dictionary<SerializableGuid, SkillBehaviourPool>();

            foreach (var skillBehaviourPool in _skillBehaviourPools)
            {
                skillBehaviourPool.InitializePool();
                _poolsByType.Add(skillBehaviourPool.ProjetileTypeId, skillBehaviourPool);
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (var skillBehaviourPool in _skillBehaviourPools)
            {
                skillBehaviourPool.UpdateActiveSkillBehaviours(deltaTime);
            }
        }

        public SkillBehaviourPool GetSkillBehaviourPoolBySkillBehaviourTypeId(SerializableGuid skillBehaviourTypeId)
        {
            if (_poolsByType.TryGetValue(skillBehaviourTypeId, out var pool))
            {
                return pool;
            }

            Debug.LogError("SkillBehaviour Pool Not Found :  " + skillBehaviourTypeId.ToHexString());
            return null;
        }

        public void ReturnAllActiveSkillBehavioursToPool()
        {
            for (int i = 0; i < _skillBehaviourPools.Count; i++)
            {
                _skillBehaviourPools[i].ReturnAllSkillBehaviours();
            }
        }
    }
}
