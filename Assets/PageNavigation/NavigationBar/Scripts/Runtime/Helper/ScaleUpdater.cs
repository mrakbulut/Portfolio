using UnityEngine;
namespace Portfolio.NavigationBars
{
    public class ScaleUpdater
    {

        private readonly Vector3 _initialScale;
        private readonly Transform _target;
        private readonly Vector3 _targetScale;

        public ScaleUpdater(Transform target, Vector3 initialScale, Vector3 targetScale)
        {
            _target = target;
            _initialScale = initialScale;
            _targetScale = targetScale;
        }

        public ScaleUpdater(Transform target, Vector3 targetScale)
        {
            _target = target;
            _initialScale = _target.localScale;
            _targetScale = targetScale;
        }

        public void SetScaleRate(float scaleRate)
        {
            _target.localScale = Vector3.Lerp(_initialScale, _targetScale, scaleRate);
        }
    }
}
