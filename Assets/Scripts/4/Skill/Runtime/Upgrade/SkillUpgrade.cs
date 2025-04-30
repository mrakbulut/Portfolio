using System.Collections.Generic;
using Portfolio.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Skill
{
    [System.Serializable]
    public class SkillUpgrade
    {
        [TitleGroup("Upgrade Info")]
        public string UpgradeName;
        [TitleGroup("Upgrade Info")]
        [TextArea(2, 4)]
        public string UpgradeDescription;

        [TitleGroup("Stat Modifiers")]
        public List<SerializedStatModifier> StatModifiers = new List<SerializedStatModifier>();
    }
}
