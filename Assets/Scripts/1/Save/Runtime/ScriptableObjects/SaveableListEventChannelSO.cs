using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Portfolio.Save
{
    [CreateAssetMenu(menuName = "Portfolio/SaveSystem/EventChannels/Saveable List Event Channel")]
    public class SaveableListEventChannelSO : ScriptableObject
    {
        public UnityAction<List<SaveableEntity>> onEventRaised;

        public void RaiseEvent(List<SaveableEntity> saveables)
        {
            if (onEventRaised != null)
            {
                onEventRaised.Invoke(saveables);
            }
        }
    }
}
