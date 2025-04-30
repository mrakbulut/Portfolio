using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Utility
{
    [CreateAssetMenu(menuName = "Portfolio/Team/TeamType")]
    public class TeamType : ScriptableObject
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
