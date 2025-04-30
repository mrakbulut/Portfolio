using Portfolio.Stats;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Skill
{
    [CreateAssetMenu(fileName = "New Skill Data", menuName = "Portfolio/Skills/Skill Data")]
    public class SkillData : ScriptableObject
    {
        public SerializableGuid Id = SerializableGuid.NewGuid();

        [Button(ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AssignNewGuid()
        {
            Id = SerializableGuid.NewGuid();
        }

        public int MaxLevel => SkillUpgrades.Length;

        [TitleGroup("Basic Info")]
        public string SkillName;
        [TitleGroup("Basic Info")]
        [TextArea(3, 5)]
        public string Description;
        [TitleGroup("Basic Info")]
        public Sprite Icon;

        [TitleGroup("Basic Info")]
        public GameObject Prefab;

        [TitleGroup("Stats")]
        public BaseStatContainer BaseStatContainer;

        [TitleGroup("Upgrades")]
        public SkillUpgrade[] SkillUpgrades;
    }
}
