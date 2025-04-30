using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.Utility
{
    public class FadeInOut : MonoBehaviour
    {
        [SerializeField] private Image _fadeInOutImage;

        private float _timer;
        private float _timeToFade;
        private bool _fading;
        private bool _fadingIn;

        private readonly Color _fadeInColor = new Color(0, 0, 0, 1f);
        private readonly Color _fadeOutColor = new Color(0, 0, 0, 0);

        private Color _from;
        private Color _to;

        public void FadeOutDirectly()
        {
            _fadeInOutImage.color = _fadeOutColor;
            _fadeInOutImage.gameObject.SetActive(false);
        }

        public void FadeInDirectly()
        {
            _fadeInOutImage.color = _fadeInColor;
            _fadeInOutImage.gameObject.SetActive(true);
        }

        public void FadeIn(float timeToFadeIn)
        {
            _timeToFade = timeToFadeIn;
            SetFading(true);
        }

        public void FadeOut(float timeToFadeOut)
        {
            _timeToFade = timeToFadeOut;
            SetFading(false);
        }

        private void SetFading(bool fadeIn)
        {
            _timer = 0f;
            _fadingIn = fadeIn;
            _fading = true;
            _fadeInOutImage.gameObject.SetActive(true);

            _from = fadeIn ? _fadeOutColor : _fadeInColor;
            _to = fadeIn ? _fadeInColor : _fadeOutColor;
        }

        private void Update()
        {
            if (!_fading) return;

            _timer += Time.deltaTime / _timeToFade;

            _fadeInOutImage.color = Color.Lerp(_from, _to, _timer);
            if (_timer >= 1f)
            {
                _fading = false;
                _fadeInOutImage.gameObject.SetActive(_fadingIn);
            }
        }
    }
}