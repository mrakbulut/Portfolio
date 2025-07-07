using UnityEngine;
namespace Portfolio.NavigationBars
{
    public class NavigationBarDynamicButtonAnchorCalculator
    {

        private const float _additionalAnchorSizeXPressedButton = 0.05f;

        private const float _notPressedButtonMaxAnchorSizeY = 1f;
        private readonly NavigationBarButton[] _buttons;
        private readonly float _normalAnchorStepX;
        private readonly float _pressedButtonMaxAnchorSizeY = 1.21f;

        public NavigationBarDynamicButtonAnchorCalculator(NavigationBarButton[] buttons, bool changeSizeYSelectedButton)
        {
            _buttons = buttons;

            _pressedButtonMaxAnchorSizeY = changeSizeYSelectedButton ? _pressedButtonMaxAnchorSizeY : 1f;
            _normalAnchorStepX = (1f - _additionalAnchorSizeXPressedButton) / _buttons.Length;
        }

        public void UpdateButtonAnchors(NavigationBarButton selectedButton)
        {
            float currentPoint = 0f;

            for (int i = 0; i < _buttons.Length; i++)
            {
                var minAnchor = new Vector2(currentPoint, 0);
                var maxAnchor = new Vector2(currentPoint + _normalAnchorStepX, _notPressedButtonMaxAnchorSizeY);

                if (_buttons[i] == selectedButton)
                {
                    if (i == 0)
                    {
                        maxAnchor.x += _additionalAnchorSizeXPressedButton;
                    }
                    else if (i == _buttons.Length - 1)
                    {
                        maxAnchor.x += _additionalAnchorSizeXPressedButton;
                    }
                    else if (selectedButton.GrowToLeft)
                    {
                        minAnchor.x -= _additionalAnchorSizeXPressedButton;
                    }
                    else
                    {
                        maxAnchor.x += _additionalAnchorSizeXPressedButton;
                    }

                    maxAnchor.y = _pressedButtonMaxAnchorSizeY;
                }

//                Debug.Log("MIN ANCHOR : " + minAnchor + ", MAX ANCHOR : " + maxAnchor + ", BUTTON : " + _buttons[i], _buttons[i]);
                _buttons[i].UpdateAnchors(minAnchor, maxAnchor);

                currentPoint = Mathf.Clamp01(maxAnchor.x);
            }
        }
    }
}
