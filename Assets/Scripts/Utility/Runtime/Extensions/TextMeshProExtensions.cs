using DG.Tweening;
using TMPro;

namespace Portfolio.Utility
{
    public static class TextMeshProExtensions
    {
        public static void ValueIncreaser(this TextMeshProUGUI valueText, int valueTarget, int valueStart, float duration, Ease ease, string frontString = null, string backString = null, float delay = 0)
        {
            int value = valueStart;
            DOTween.To(() => value, x => value = x, valueTarget, duration)
                .SetEase(ease)
                .SetDelay(delay)
                .OnUpdate(() =>
                {
                    valueText.text = frontString + value.ToString() + backString;
                });

        }
    }
}
