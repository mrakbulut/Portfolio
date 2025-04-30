using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.Utility
{
    public static class ImageExtensions
    {
        public static void FillAmountAnimation(this Image image, float targetFillAmount, float duration, Ease ease)
        {
            DOTween.To(() => image.fillAmount, x => image.fillAmount = x, targetFillAmount, duration).SetEase(ease);
        }
        public static void ColorAnimation(this Image image, Color minColor, Color maxColor, float duration, Ease ease)
        {
            float fillAmount = image.fillAmount;
            var targetColor = Color.Lerp(minColor, maxColor, fillAmount);

            DOTween.To(() => image.color, x => image.color = x, targetColor, duration).SetEase(ease);
        }
    }
}
