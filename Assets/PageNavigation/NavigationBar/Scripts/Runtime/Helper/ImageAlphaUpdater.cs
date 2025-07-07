using UnityEngine;
using UnityEngine.UI;
namespace Portfolio.NavigationBars
{
    public class ImageAlphaUpdater
    {
        private readonly Image _image;

        private readonly float _initialAlpha;
        private readonly float _targetAlpha;

        public ImageAlphaUpdater(Image image, float initialAlpha, float targetAlpha)
        {
            _image = image;
            _initialAlpha = initialAlpha;
            _targetAlpha = targetAlpha;
        }

        public ImageAlphaUpdater(Image image, float targetAlpha)
        {
            _image = image;
            _initialAlpha = image.color.a;
            _targetAlpha = targetAlpha;
        }

        public void SetAlphaRate(float rate)
        {
            var color = _image.color;
            color.a = Mathf.Lerp(_initialAlpha, _targetAlpha, rate);
            //Debug.Log("COLOR ALPHA : " + color.a, _image.gameObject);
            _image.color = color;
        }
    }
}
