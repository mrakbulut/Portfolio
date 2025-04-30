using UnityEngine;
using UnityEngine.Events;

namespace Portfolio.Utility
{
    [CreateAssetMenu(menuName = "Portfolio/EventChannels/FloatEventChannel")]
    public class FloatEventChannel : ScriptableObject
    {
        public UnityAction<float> OnEventRaise;

        public void RaiseEvent(float value)
        {
            OnEventRaise?.Invoke(value);
        }
    }
}
