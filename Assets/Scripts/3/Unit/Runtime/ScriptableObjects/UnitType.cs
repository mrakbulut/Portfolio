using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Unit
{
    [CreateAssetMenu(fileName = "UnitType", menuName = "Portfolio/Unit/UnitType")]
    public class UnitType : ScriptableObject
    {
        [SerializeField]
        public SerializableGuid Id = SerializableGuid.NewGuid();

        [Button(ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AssignNewGuid()
        {
            Id = SerializableGuid.NewGuid();
        }
    }
}
