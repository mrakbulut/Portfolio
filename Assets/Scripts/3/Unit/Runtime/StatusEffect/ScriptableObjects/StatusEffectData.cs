using System.Collections.Generic;
using Portfolio.Stats;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Unit
{
    [CreateAssetMenu(fileName = "StatusEffectData", menuName = "Portfolio/StatusEffectData")]
    public class StatusEffectData : ScriptableObject
    {
        [Button(ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AssignNewGuid()
        {
            Id = SerializableGuid.NewGuid();
        }
        public SerializableGuid Id = SerializableGuid.NewGuid();
        public string Name;
        public string Description;
        public Sprite Icon;
        public bool IsStackable;
        public bool IsPermanent;
        public List<StatusEffectLevelData> Levels = new List<StatusEffectLevelData>();

        public StatusEffectLevelData GetLevelData(int level)
        {
            return Levels.Find(l => l.Level == level) ?? Levels[0];
        }

        [System.Serializable]
        public class StatusEffectLevelData
        {
            public int Level;
            public List<StatTypeStatModifier> StatTypeStatModifiers;
        }

        [System.Serializable]
        public class StatTypeStatModifier
        {
            public StatType StatType;
            public StatModifier StatModifier;
        }
    }
}
