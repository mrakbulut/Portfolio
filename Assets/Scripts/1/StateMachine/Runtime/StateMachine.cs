namespace Portfolio.StateMachines
{
    public class StateMachine
    {
        private IState _currentState;

        public void SwitchState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }

        public void Tick(float deltaTime)
        {
            _currentState?.Tick(deltaTime);
        }
    }
}
