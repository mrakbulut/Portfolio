using System;
using System.Collections.Generic;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Portfolio.Skill
{
    [Serializable]
    public class SkillBehaviourPool
    {
        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private Transform _parent;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private GameObject _skillBehaviourPrefab;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private SkillBehaviourType _skillBehaviourType;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _initialPoolSize = 20;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _maxPoolSize = 40;

        private SerializableGuid _skillBehaviourTypeId;
        public SerializableGuid ProjetileTypeId => _skillBehaviourTypeId;

        private IObjectPool<ISkillBehaviour> _pool;

        private readonly List<ISkillBehaviour> _activeSkillBehaviours = new List<ISkillBehaviour>();

        public void InitializePool()
        {
            _skillBehaviourTypeId = _skillBehaviourType.Id;

            _pool = new ObjectPool<ISkillBehaviour>(
                CreateSkillBehaviour,
                OnSkillBehaviourRetrieved,
                OnSkillBehaviourReturned,
                OnSkillBehaviourDestroyed,
                true,
                _initialPoolSize,
                _maxPoolSize);
        }

        private ISkillBehaviour CreateSkillBehaviour()
        {
            var skillBehaviourGO = Object.Instantiate(_skillBehaviourPrefab, _parent);
            var skillBehaviour = skillBehaviourGO.GetComponent<ISkillBehaviour>();
            skillBehaviour.Initialize(_pool);
            return skillBehaviour;
        }

        private void OnSkillBehaviourRetrieved(ISkillBehaviour skillBehaviour)
        {
            skillBehaviour.Show();
            _activeSkillBehaviours.Add(skillBehaviour);
        }

        private void OnSkillBehaviourReturned(ISkillBehaviour skillBehaviour)
        {
            skillBehaviour.Hide(_parent);
            skillBehaviour.Refresh();
            _activeSkillBehaviours.Remove(skillBehaviour);
        }

        private void OnSkillBehaviourDestroyed(ISkillBehaviour skillBehaviour)
        {

        }

        public ISkillBehaviour GetSkillBehaviour()
        {
            var skillBehaviour = _pool.Get();
            skillBehaviour.Initialize(_pool);
            //skillBehaviour.SetPosition(position);
            return skillBehaviour;
        }

        public void UpdateActiveSkillBehaviours(float deltaTime)
        {
            for (int i = _activeSkillBehaviours.Count - 1; i >= 0; i--)
            {
                //_activeSkillBehaviours[i].Update(deltaTime);
            }
        }

        public void ReturnAllSkillBehaviours()
        {
            for (int i = _activeSkillBehaviours.Count - 1; i >= 0; i--)
            {
                _activeSkillBehaviours[i].ReturnToPool();
            }
        }
    }
}
