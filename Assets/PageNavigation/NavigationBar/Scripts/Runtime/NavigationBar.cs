using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Portfolio.NavigationBars
{
    public class NavigationBar : MonoBehaviour
    {
        [SerializeField] private NavigationBarButton[] _buttons;
        [SerializeField] private int _initialSelectedButtonIndex;
        [SerializeField] private bool _changeSizeYSelectedButton;

        private NavigationBarButton _currentButton;

        private NavigationBarDynamicButtonAnchorCalculator _dynamicButtonAnchorCalculator;

        public Action<NavigationBarButton, int> OnSelectedButtonChanged = (_, _) => { };
        public int SelectedButtonIndex => _currentButton != null ? _currentButton.PageIndex : -1;

        private void Awake()
        {
            _dynamicButtonAnchorCalculator = new NavigationBarDynamicButtonAnchorCalculator(_buttons, _changeSizeYSelectedButton);

            InitializeButtons();
        }

        #if UNITY_EDITOR
        [TitleGroup("Only Editor")]
        [Button(ButtonSizes.Large, ButtonStyle.Box)] [GUIColor(0.4f, 0.8f, 1)]
        private void PlaceAllButtonsByDefault()
        {
            var dynamicButtonAnchorCalculator = new NavigationBarDynamicButtonAnchorCalculator(_buttons, _changeSizeYSelectedButton);
            var selectedButton = _buttons[Mathf.Clamp(_initialSelectedButtonIndex, 0, _buttons.Length - 1)];
            dynamicButtonAnchorCalculator.UpdateButtonAnchors(selectedButton);

            for (int i = 0; i < _buttons.Length; i++)
            {
                var navigationButtonStateMachine = _buttons[i].GetComponent<NavigationButtonStateMachine>();
                if (selectedButton != _buttons[i])
                {

                    navigationButtonStateMachine.SetNotPressedDirectly(_buttons[i].MinAnchor, _buttons[i].MaxAnchor);
                }
                else
                {
                    navigationButtonStateMachine.SetPressedDirectly(_buttons[i].MinAnchor, _buttons[i].MaxAnchor);
                }

                var buttonTransform = _buttons[i].transform as RectTransform;
                if (buttonTransform == null) continue;

                buttonTransform.anchoredPosition = Vector2.zero;
                buttonTransform.sizeDelta = Vector2.zero;
            }
        }
        #endif

        private void InitializeButtons()
        {
            var selectedButton = _buttons[Mathf.Clamp(_initialSelectedButtonIndex, 0, _buttons.Length - 1)];
            _dynamicButtonAnchorCalculator.UpdateButtonAnchors(selectedButton);

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Initialize(i);
                _buttons[i].OnButtonClicked += HandleNavigationButtonClick;
                if (selectedButton != _buttons[i]) _buttons[i].SetNotPressedDirectly();
            }

            SetSelectedButton(_initialSelectedButtonIndex, true);
        }
        private void HandleNavigationButtonClick(NavigationBarButton navigationBarButton, int pageIndex)
        {
            SetSelectedButton(navigationBarButton);
        }

        private void SetSelectedButton(int buttonIndex, bool setDirectly = false)
        {
            _initialSelectedButtonIndex = Mathf.Clamp(buttonIndex, 0, _buttons.Length - 1);

            SetSelectedButton(_buttons[buttonIndex], setDirectly);
        }

        private void SetSelectedButton(NavigationBarButton selectedNavigationBarButton, bool setDirectly = false)
        {
            if (_currentButton == selectedNavigationBarButton) return;

            _dynamicButtonAnchorCalculator.UpdateButtonAnchors(selectedNavigationBarButton);

            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == selectedNavigationBarButton) continue;

                if (setDirectly)
                {
                    _buttons[i].SetNotPressedDirectly();
                }
                else
                {
                    _buttons[i].SetNotPressed();
                }
            }

            _currentButton = selectedNavigationBarButton;
            if (setDirectly)
            {
                _currentButton.SetPressedDirectly();
            }
            else
            {
                _currentButton.SetPressed();
            }

            OnSelectedButtonChanged.Invoke(_currentButton, _currentButton.PageIndex);
        }
    }
}
