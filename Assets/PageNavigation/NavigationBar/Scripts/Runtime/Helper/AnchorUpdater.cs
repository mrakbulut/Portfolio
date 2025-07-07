using UnityEngine;
namespace Portfolio.NavigationBars
{
    public class AnchorUpdater
    {
        private readonly Vector2 _initialMaxAnchor;

        private readonly Vector2 _initialMinAnchor;
        private readonly RectTransform _target;
        private readonly Vector2 _targetMaxAnchor;

        private readonly Vector2 _targetMinAnchor;

        public AnchorUpdater(RectTransform target, Vector2 initialMinAnchor, Vector2 initialMaxAnchor, Vector2 targetMinAnchor, Vector2 targetMaxAnchor)
        {
            _target = target;

            _initialMinAnchor = initialMinAnchor;
            _initialMaxAnchor = initialMaxAnchor;
            _targetMinAnchor = targetMinAnchor;
            _targetMaxAnchor = targetMaxAnchor;
        }

        public AnchorUpdater(RectTransform target, Vector2 targetMinAnchor, Vector2 targetMaxAnchor)
        {
            _target = target;

            _initialMinAnchor = target.anchorMin;
            _initialMaxAnchor = target.anchorMax;

            _targetMinAnchor = targetMinAnchor;
            _targetMaxAnchor = targetMaxAnchor;
        }

        public void SetAnchorRate(float rate)
        {
            _target.anchorMin = Vector2.Lerp(_initialMinAnchor, _targetMinAnchor, rate);
            _target.anchorMax = Vector2.Lerp(_initialMaxAnchor, _targetMaxAnchor, rate);
        }

        public void SetDirectly(Vector2 minAnchor, Vector2 maxAnchor)
        {
            _target.anchorMin = minAnchor;
            _target.anchorMax = maxAnchor;
        }
    }
}
