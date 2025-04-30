using Portfolio.StateMachines;
using UnityEngine;

namespace Portfolio.Skill
{
    public class IdleState : IState
    {
        private readonly SkillBase _skill;

        public IdleState(SkillBase skill)
        {
            _skill = skill;
        }

        public void Enter()
        {
            #if UNITY_EDITOR
            Debug.Log("Entering Idle State");
            #endif
        }

        public void Exit()
        {
            // Nothing to clean up
        }

        public void Tick(float deltaTime)
        {
        }
    }
}
