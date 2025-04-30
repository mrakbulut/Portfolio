using System;
using System.Collections.Generic;
using Portfolio.Stats;
using Portfolio.StateMachines;
using Portfolio.Unit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Skill
{
    public abstract class SkillBase : MonoBehaviour
    {
        [TitleGroup("Base Props")]
        [SerializeField] protected SkillData skillData;

        protected StatContainer statContainer;
        protected StateMachine stateMachine;
        protected IUnit ownerUnit;
        protected GameObject owner;
        protected IStatContainer ownerStatContainer;

        protected int level;
        protected bool initialized;

        protected readonly List<SkillUpgrade> appliedUpgrades = new List<SkillUpgrade>();

        public bool Active { get; protected set; }
        public bool IsOnCooldown { get; protected set; }
        public bool Executing { get; protected set; }

        public Action<bool> OnActiveStatusChanged = _ => { };

        private void Awake()
        {
            statContainer = new StatContainer(skillData.BaseStatContainer);
            stateMachine = new StateMachine();
        }

        public virtual void Initialize(IUnit ownerUnit, GameObject skillOwner, IStatContainer ownerStats)
        {
            if (initialized) return;

            this.ownerUnit = ownerUnit;
            owner = skillOwner;
            //Debug.Log("SKILL OWNER : " + owner, owner);
            ownerStatContainer = ownerStats;

            if (skillData != null)
            {
                statContainer = new StatContainer(skillData.BaseStatContainer);
            }
            else
            {
                Debug.LogError("Skill Data is not assigned to the skill: " + gameObject.name);
            }

            ApplySkillUpgrade();
            OnInitialized();
            initialized = true;
        }

        protected virtual void Update()
        {
            if (!initialized) return;

            stateMachine.Tick(Time.deltaTime);
        }

        protected abstract void OnInitialized();

        public virtual void LevelUp()
        {
            if (level >= skillData.MaxLevel - 1) return;

            level++;
            //Debug.Log("SKILL LEVEL : " + level, gameObject);
            ApplySkillUpgrade();

            OnLevelChanged();
        }

        private void ApplySkillUpgrade()
        {
            if (level >= skillData.SkillUpgrades.Length)
            {
                Debug.LogWarning("YOU CANNOT UPGRADE THE SKILL CAUSE SKILL HAS BEEN REACHED AT MAX LEVEL.", skillData);
                return;
            }

            var upgrade = skillData.SkillUpgrades[level];
            if (appliedUpgrades.Contains(upgrade))
            {
                Debug.LogError("YOU CANNOT APPLY THE SAME UPGRADE MORE THAN ONCE ! SKILL LEVEL : " + level, skillData);
                return;
            }

            statContainer.RemoveModifiersFromSource(this);

            ApplyModifiers(upgrade.StatModifiers);

            appliedUpgrades.Add(upgrade);
        }

        protected abstract void OnLevelChanged();

        private void ApplyModifiers(List<SerializedStatModifier> serializedStatModifiers)
        {
            for (int i = 0; i < serializedStatModifiers.Count; i++)
            {
                var serializedModifier = serializedStatModifiers[i];
                var modifier = serializedModifier.CreateModifier(this);

                var stat = statContainer.GetStatByStatTypeId(serializedModifier.StatType.Id);
                if (stat is null)
                {
                    Debug.LogError("WANT TO MODIFY AN UNDEFINED STAT");
                    continue;
                }

                stat.AddModifier(modifier);
            }
        }

        public abstract void Activate();
        public abstract void Deactivate();
    }
}
