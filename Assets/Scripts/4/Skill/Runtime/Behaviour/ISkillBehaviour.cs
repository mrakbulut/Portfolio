using Portfolio.Stats;
using Portfolio.Unit;
using Portfolio.Utility;
using UnityEngine;
using UnityEngine.Pool;

namespace Portfolio.Skill
{
    public interface ISkillBehaviour
    {
        SerializableGuid SkillBehaviourTypeId { get; }
        void Initialize(IObjectPool<ISkillBehaviour> pool);
        void Setup(IUnit owner, Transform ownerTransform, IStatContainer ownerStatContainer, IStatContainer skillStatContainer, int skillLevel);
        void Execute(System.Action<ISkillBehaviour> onExecutionComplete);
        void Remove();
        void Refresh();
        void Hide(Transform parent);
        void Show();
        void ReturnToPool();
    }
}
