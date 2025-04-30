using System;
using Portfolio.Stats;
using Portfolio.Upgradeables;
using Portfolio.Utility;

namespace Portfolio.Skill
{
    public class SkillUpgradeable : IUpgradeable
    {
        private readonly SkillData _skillData;

        public bool Active { get; private set; }
        public int Level { get; private set; }
        public bool ReachedMaxLevel { get; private set; }

        public event Action<IUpgradeable> OnActivated = _ => { };
        public event Action<IUpgradeable> OnUpgraded = _ => { };

        public SkillData SkillData => _skillData;

        public SkillUpgradeable(SkillData skillData)
        {
            _skillData = skillData;
            Active = false;
            Level = 0;
            ReachedMaxLevel = false;
        }

        public void Upgrade()
        {
            if (Active)
            {
                Level++;
                //Debug.Log("UPGRADED SKILL LEVEL : " + Level + ", MAX LEVEL : " + _skillData.MaxLevel);
                if (Level >= _skillData.MaxLevel)
                {
                    ReachedMaxLevel = true;
                }
                OnUpgraded.Invoke(this);
            }
            else
            {
                //Debug.Log("ACTIVATING SKILL : " + _skillData.SkillName, _skillData);
                Level++;
                Active = true;
                OnActivated.Invoke(this);
            }
        }

        public UpgradeSummaryData GetUpgradeSummaryData()
        {
            if (ReachedMaxLevel) return new UpgradeSummaryData(string.Empty, string.Empty);

            string difference = string.Empty;
            string title = _skillData.SkillName;

            if (Active)
            {
                title += " LEVEL " + Level;
                var currentUpgrades = _skillData.SkillUpgrades[Level - 1];
                var nextLevelUpgrades = _skillData.SkillUpgrades[Level];
                difference = GetSkillNextLevelUpgradeDescription(currentUpgrades, nextLevelUpgrades);
            }
            else
            {
                var upgrade = _skillData.SkillUpgrades[Level];
                difference = GetSkillUpgradeDescription(upgrade);
            }

            return new UpgradeSummaryData(title, difference);
        }

        private string GetSkillUpgradeDescription(SkillUpgrade skillUpgrade)
        {
            string description = string.Empty;

            for (int i = 0; i < skillUpgrade.StatModifiers.Count; i++)
            {
                description += GetStatModifierDescription(skillUpgrade.StatModifiers[i]);
                if (i != skillUpgrade.StatModifiers.Count - 1)
                {
                    description += "\n";
                }
            }

            return description;
        }

        private string GetStatModifierDescription(SerializedStatModifier statModifier)
        {
            return statModifier.StatType.DisplayName + " : " + statModifier.Value;
        }

        private string GetSkillNextLevelUpgradeDescription(SkillUpgrade currentSkillUpgrade, SkillUpgrade nextSkillUpgrade)
        {
            string description = string.Empty;

            for (int i = 0; i < currentSkillUpgrade.StatModifiers.Count; i++)
            {
                var currentStatModifier = currentSkillUpgrade.StatModifiers[i];
                var nextStatModifier = nextSkillUpgrade.StatModifiers[i];

                float difference = nextStatModifier.Value - currentStatModifier.Value;
                if (difference == 0) continue;

                if (i > 0)
                {
                    description += "\n";
                }

                description += GetCurrentAndNextStatModifierDifferenceDescription(currentStatModifier, nextStatModifier);
            }

            return description;
        }

        private string GetCurrentAndNextStatModifierDifferenceDescription(SerializedStatModifier currentStatModifier, SerializedStatModifier nextStatModifier)
        {
            float difference = nextStatModifier.Value - currentStatModifier.Value;
            string sign = difference > 0 ? "+" : "-";

            return currentStatModifier.StatType.DisplayName + " : <color=\"green\">" + sign + difference.ToString().Replace(',', '.') + "</color>";
        }

        private SerializedStatModifier GetSkillModifier(SkillUpgrade skillUpgrade, SerializableGuid statTypeId)
        {
            for (int i = 0; i < skillUpgrade.StatModifiers.Count; i++)
            {
                var statModifier = skillUpgrade.StatModifiers[i];
                if (statModifier.StatType.Id == statTypeId)
                {
                    return statModifier;
                }
            }

            return null;
        }
    }
}
