using Portfolio.StateMachines;
using UnityEngine;
using UnityEngine.UI;
namespace Portfolio.NavigationBars
{
    public class NavigationButtonStateMachine : StateMachine
    {
        private readonly ButtonImageAnchorUpdater _imageAnchorUpdater;
        private readonly Image _pressedBackgroundImage;
        private readonly RectTransform _target;
        private readonly Transform _textTransform;

        public NavigationButtonStateMachine(ButtonImageAnchorUpdater imageAnchorUpdater, Image pressedBackgroundImage, RectTransform target, Transform textTransform)
        {
            _imageAnchorUpdater = imageAnchorUpdater;
            _pressedBackgroundImage = pressedBackgroundImage;
            _target = target;
            _textTransform = textTransform;
        }

        public void Idle()
        {
            var idle = new NavigationButtonIdleState(this);
            SwitchState(idle);
        }

        public void Pressed(Vector2 minAnchor, Vector2 maxAnchor)
        {
            var pressed = new NavigationButtonPressedState(_textTransform, _imageAnchorUpdater.GenerateToUpperAnchorUpdater(), _target, _pressedBackgroundImage, minAnchor, maxAnchor, Idle);
            SwitchState(pressed);
        }

        public void NotPressed(Vector2 minAnchor, Vector2 maxAnchor)
        {
            var notPressed = new NavigationButtonNotPressedState(_textTransform, _imageAnchorUpdater.GenerateToLowerAnchorUpdater(), _target, _pressedBackgroundImage, minAnchor, maxAnchor, Idle);
            SwitchState(notPressed);
        }

        public void SetPressedDirectly(Vector2 minAnchor, Vector2 maxAnchor)
        {
            _target.anchorMin = minAnchor;
            _target.anchorMax = maxAnchor;

            _imageAnchorUpdater.SetToUpperAnchorDirectly();
            _textTransform.localScale = Vector3.one;

            SetBackgroundAlpha(1f);
        }
        public void SetNotPressedDirectly(Vector2 minAnchor, Vector2 maxAnchor)
        {
            _target.anchorMin = minAnchor;
            _target.anchorMax = maxAnchor;

            _imageAnchorUpdater.SetToLowerAnchorDirectly();
            _textTransform.localScale = Vector3.zero;

            SetBackgroundAlpha(0f);
        }


        private void SetBackgroundAlpha(float alpha)
        {
            var color = _pressedBackgroundImage.color;
            color.a = alpha;
            _pressedBackgroundImage.color = color;
        }
    }
}
