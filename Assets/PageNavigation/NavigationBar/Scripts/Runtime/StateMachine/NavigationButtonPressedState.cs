using System;
using Portfolio.StateMachines;
using UnityEngine;
using UnityEngine.UI;
namespace Portfolio.NavigationBars
{
    public class NavigationButtonPressedState : IState
    {

        private const float _timeToChange = .25f;
        private readonly ImageAlphaUpdater _backgroundAlphaUpdater;
        private readonly AnchorUpdater _iconAnchorUpdater;
        private readonly AnchorUpdater _navigationBarButtonAnchorUpdater;
        private readonly ScaleUpdater _textScaleUpdater;
        private bool _active;

        private float _timer;

        public Action OnComplete = () => { };

        public NavigationButtonPressedState(Transform textTransform, AnchorUpdater iconAnchorUpdater, RectTransform target, Image pressedBackgroundImage, Vector2 targetMinAnchor, Vector2 targetMaxAnchor, Action onComplete)
        {
            _iconAnchorUpdater = iconAnchorUpdater;
            _textScaleUpdater = new ScaleUpdater(textTransform, Vector3.one);
            _navigationBarButtonAnchorUpdater = new AnchorUpdater(target, targetMinAnchor, targetMaxAnchor);
            _backgroundAlphaUpdater = new ImageAlphaUpdater(pressedBackgroundImage, 1f);

            OnComplete += onComplete;
        }
        public void Enter()
        {
            _active = true;
            _timer = 0f;
            //Debug.Log("PRESSED BUTTON : " + stateMachine.gameObject, stateMachine.gameObject);
        }
        public void Exit()
        {
            _active = false;
            //Debug.Log("PRESSED EXIT : " + stateMachine.gameObject, stateMachine.gameObject);
        }
        public void Tick(float deltaTime)
        {
            if (!_active) return;

            _timer += deltaTime / _timeToChange;

            _navigationBarButtonAnchorUpdater.SetAnchorRate(_timer);
            _iconAnchorUpdater.SetAnchorRate(_timer);
            _textScaleUpdater.SetScaleRate(_timer);
            _backgroundAlphaUpdater.SetAlphaRate(_timer);

            //Debug.Log("TARGET ANCHOR MIN : " + _target.anchorMin + ", TARGET ANCHOR MAX : " + _target.anchorMax, stateMachine.gameObject);

            if (_timer >= 1f)
            {
                Complete();
            }
        }

        private void Complete()
        {
            _active = false;
            OnComplete.Invoke();
        }
    }
}
