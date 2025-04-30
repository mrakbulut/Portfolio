using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Projectile
{
    [CreateAssetMenu(fileName = "New Projectile Type", menuName = "Portfolio/Projectile/ProjectileType")]
    public class ProjectileType : ScriptableObject
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
