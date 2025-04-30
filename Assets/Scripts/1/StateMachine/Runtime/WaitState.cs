namespace Portfolio.StateMachines
{
    public class WaitState : IState
    {
        private readonly float _duration;

        private System.Action _onComplete = () => { };

        private float _timer;
        private bool _completed;

        public WaitState(float duration, System.Action onComplete)
        {
            _duration = duration;

            _onComplete += onComplete;
        }

        public void Enter()
        {
            _timer = 0f;
            _completed = false;
        }
        public void Exit()
        {
            _onComplete = () => { };
        }
        public void Tick(float deltaTime)
        {
            if (_completed) return;

            _timer += deltaTime;
            if (_timer >= _duration)
            {
                Complete();
            }
        }
        private void Complete()
        {
            _completed = true;
            _onComplete.Invoke();
        }
    }
}
