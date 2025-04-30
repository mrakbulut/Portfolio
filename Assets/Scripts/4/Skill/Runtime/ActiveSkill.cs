using Portfolio.Stats;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Skill
{
    public class ActiveSkill : SkillBase
    {
        [TitleGroup("Active Skill Properties")]
        [SerializeField] private SkillBehaviourType _skillBehaviourType;

        [TitleGroup("Active Skill Properties")]
        [SerializeField] private StatType _cooldownStatType;

        [TitleGroup("Active Skill Properties/Settings")]
        [SerializeField] private bool _startCooldownAfterExecutionComplete;

        private Stat _cooldownStat;
        private SkillBehaviourPool _skillBehaviourPool;

        private SkillCooldownManager _cooldownManager;
        public SkillCooldownManager CooldownManager => _cooldownManager;

        public System.Action OnSkillExecutionStart = () => { };

        protected override void OnInitialized()
        {
            _cooldownStat = statContainer.GetStatByStatType(_cooldownStatType);

            _cooldownManager = new SkillCooldownManager(_cooldownStat);

            var skillBehaviourPoolManager = ServiceLocator.Instance.Get<SkillBehaviourPoolManager>();
            _skillBehaviourPool = skillBehaviourPoolManager.GetSkillBehaviourPoolBySkillBehaviourTypeId(_skillBehaviourType.Id);
        }

        protected override void OnLevelChanged() { } // TODO: Update Skill Behaviour ?

        private void ExecuteSkill()
        {
            if (!Active || IsOnCooldown) return;

            OnSkillExecutionStart.Invoke();
            var skillBehaviour = _skillBehaviourPool.GetSkillBehaviour();
            //Debug.Log("OWNER : " + owner, owner);
            skillBehaviour.Setup(ownerUnit, owner.transform, ownerStatContainer, statContainer, level);
            skillBehaviour.Execute(OnSkillBehaviourExecutionComplete);
            Executing = true;
        }

        private void OnSkillBehaviourExecutionComplete(ISkillBehaviour behaviour)
        {
            if (_startCooldownAfterExecutionComplete)
            {
                StartCooldown();
            }
        }

        public override void Activate()
        {
            if (Active) return;

            Active = true;
            OnActiveStatusChanged.Invoke(Active);
            StartCooldown();
        }

        public override void Deactivate()
        {
            if (!Active) return;

            Active = false;
            OnActiveStatusChanged.Invoke(Active);
            Idle();
        }

        private void Idle()
        {
            var idle = new IdleState(this);
            stateMachine.SwitchState(idle);
        }

        private void StartCooldown()
        {
            Executing = false;
            IsOnCooldown = true;

            var cooldownState = new WaitForCooldownState(
                this,
                _cooldownManager,
                OnCooldownFinished
                );

            stateMachine.SwitchState(cooldownState);
        }

        private void OnCooldownFinished()
        {
            IsOnCooldown = false;

            ExecuteSkill();

            Idle();

            if (!_startCooldownAfterExecutionComplete)
            {
                StartCooldown();
            }

        }
    }
}
