using Portfolio.Upgradeables;
using TMPro;
using UnityEngine;

namespace Portfolio.RogueLikeUpgrade
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public void Setup(UpgradeSummaryData upgradeSummary)
        {
            _titleText.text = upgradeSummary.Title;
            _descriptionText.text = upgradeSummary.Description;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
