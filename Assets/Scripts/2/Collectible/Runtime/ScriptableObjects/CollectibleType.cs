using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Collectible
{
    [CreateAssetMenu(fileName = "CollectibleType", menuName = "Portfolio/Collectible/CollectibleType")]
    public class CollectibleType : ScriptableObject
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
