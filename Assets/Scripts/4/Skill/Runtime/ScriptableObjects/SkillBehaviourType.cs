using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Skill
{
    [CreateAssetMenu(fileName = "SkillBehaviourType", menuName = "Portfolio/Skills/SkillBehaviourType")]
    public class SkillBehaviourType : ScriptableObject
    {
        public SerializableGuid Id = SerializableGuid.NewGuid();

        [Button(ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AssignNewGuid()
        {
            Id = SerializableGuid.NewGuid();
        }
    }
}
