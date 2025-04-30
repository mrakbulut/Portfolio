using Portfolio.StateMachines;

namespace Portfolio.Skill
{
    public class WaitForCooldownState : IState
    {
        private readonly SkillBase _skill;
        private readonly SkillCooldownManager _cooldownManager;

        private bool _finished;

        private System.Action _onCooldownFinished = () => { };

        public WaitForCooldownState(SkillBase skill, SkillCooldownManager skillCooldownManager, System.Action onCooldownFinished)
        {
            _skill = skill;

            _cooldownManager = skillCooldownManager;
            _onCooldownFinished += onCooldownFinished;
        }

        public void Enter()
        {
            #if UNITY_EDITOR
            //Debug.Log("Entering Cooldown State");
            #endif

            _cooldownManager.StartCooldown();
        }

        public void Exit()
        {
            // Nothing to clean up
        }

        public void Tick(float deltaTime)
        {
            if (_finished) return;

            if (!_skill.Active)
            {
                return;
            }

            bool finished = _cooldownManager.Update(deltaTime);
            if (finished)
            {
                FinishCooldown();
            }

        }

        private void FinishCooldown()
        {
            if (_finished) return;

            _finished = true;
            _onCooldownFinished.Invoke();
        }
    }
}
