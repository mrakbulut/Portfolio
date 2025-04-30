using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Stats
{
    [CreateAssetMenu(fileName = "StatType", menuName = "Portfolio/Stat/StatType")]
    public class StatType : ScriptableObject
    {
        public SerializableGuid Id = SerializableGuid.NewGuid();

        public string DisplayName;

        [Button(ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AssignNewGuid()
        {
            Id = SerializableGuid.NewGuid();
        }
    }
}
