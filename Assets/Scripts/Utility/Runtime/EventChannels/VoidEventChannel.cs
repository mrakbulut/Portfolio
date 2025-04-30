using UnityEngine;
using UnityEngine.Events;

namespace Portfolio.Utility
{
    [CreateAssetMenu(menuName = "Portfolio/EventChannels/VoidEventChannel")]
    public class VoidEventChannel : ScriptableObject
    {
        public UnityAction OnEventRaise = () => { };

        public void RaiseEvent()
        {
            OnEventRaise.Invoke();
        }
    }
}
