using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
namespace Portfolio.NavigationBars
{
    public class NavigationBarButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _growToLeft;
        [SerializeField] private Image _pressedBackgroundImage;
        [SerializeField] private RectTransform _target;
        [SerializeField] private Transform _textTransform;

        [TitleGroup("State Machine Props")]
        private ButtonImageAnchorUpdater _imageAnchorUpdater;


        private NavigationButtonStateMachine _stateMachine;

        public Action<NavigationBarButton, int> OnButtonClicked = (_, _) => { };
        public bool GrowToLeft => _growToLeft;
        public int PageIndex
        {
            get;
            private set;
        }
        public Vector2 MinAnchor
        {
            get;
            private set;
        }
        public Vector2 MaxAnchor
        {
            get;
            private set;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            _stateMachine.Tick(deltaTime);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ButtonClicked);
        }

        public void Initialize(int pageIndex)
        {
            PageIndex = pageIndex;

            _stateMachine = new NavigationButtonStateMachine(_imageAnchorUpdater, _pressedBackgroundImage, _target, _textTransform);
            _button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            OnButtonClicked(this, PageIndex);
        }

        public void SetPressed()
        {
            //Debug.Log("PRESSED : " + gameObject, gameObject);
            _stateMachine.Pressed(MinAnchor, MaxAnchor);
        }

        public void SetNotPressed()
        {
            //Debug.Log("NOT PRESSED : " + gameObject, gameObject);
            _stateMachine.NotPressed(MinAnchor, MaxAnchor);
        }

        public void SetNotPressedDirectly()
        {
            _stateMachine.SetNotPressedDirectly(MinAnchor, MaxAnchor);
        }

        public void SetPressedDirectly()
        {
            _stateMachine.SetPressedDirectly(MinAnchor, MaxAnchor);
        }

        public void UpdateAnchors(Vector2 minAnchor, Vector2 maxAnchor)
        {
            MinAnchor = minAnchor;
            MaxAnchor = maxAnchor;
        }
    }


}
