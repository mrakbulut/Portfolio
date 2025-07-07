using Sirenix.OdinInspector;
using UnityEngine;
namespace Portfolio.NavigationBars
{
    public class ButtonImageAnchorUpdater : MonoBehaviour
    {
        [SerializeField] private RectTransform _target;

        [TabGroup("Lower Anchor")] [SerializeField] private Vector2 _lowerAnchorMin;
        [TabGroup("Lower Anchor")] [SerializeField] private Vector2 _lowerAnchorMax;

        [TabGroup("Upper Anchor")] [SerializeField] private Vector2 _upperAnchorMin;
        [TabGroup("Upper Anchor")] [SerializeField] private Vector2 _upperAnchorMax;

        public void SetToLowerAnchorDirectly()
        {
            _target.anchorMin = _lowerAnchorMin;
            _target.anchorMax = _lowerAnchorMax;
        }

        public void SetToUpperAnchorDirectly()
        {
            _target.anchorMin = _upperAnchorMin;
            _target.anchorMax = _upperAnchorMax;
        }

        public AnchorUpdater GenerateToLowerAnchorUpdater()
        {
            return new AnchorUpdater(_target, _lowerAnchorMin, _lowerAnchorMax);
        }
        public AnchorUpdater GenerateToUpperAnchorUpdater()
        {
            return new AnchorUpdater(_target, _upperAnchorMin, _upperAnchorMax);
        }
    }
}
