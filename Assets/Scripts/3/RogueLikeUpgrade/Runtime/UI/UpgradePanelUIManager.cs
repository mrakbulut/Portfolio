using System;
using Portfolio.Upgradeables;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.RogueLikeUpgrade
{
    public class UpgradePanelUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _panelUI;
        [SerializeField] private UpgradeUI[] _upgradeUIs;

        [TitleGroup("Upgrade Panel Animation")]
        [SerializeField] private float _duration = .2f;

        public Action<int> OnUpgradeSelected = _ => { };

        private void Awake()
        {
            _panelUI.SetActive(false);
        }

        public void ShowPanel(IUpgradeable[] upgrades)
        {
            // TODO: Get Upgrades as a parameter
            _panelUI.SetActive(true);

            ShowPanelAnimation();
            SetupAndShowUpgrades(upgrades);
        }

        private void ShowPanelAnimation()
        {
            _panelUI.transform.localPosition = Vector3.up * 500f;
            _panelUI.transform
                .DOLocalMove(Vector3.zero, _duration)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }

        private void SetupAndShowUpgrades(IUpgradeable[] upgrades)
        {
            for (int i = 0; i < _upgradeUIs.Length; i++)
            {
                if (upgrades.Length <= i)
                {
                    _upgradeUIs[i].Hide();
                }
                else
                {
                    _upgradeUIs[i].Setup(upgrades[i].GetUpgradeSummaryData());
                    _upgradeUIs[i].Show();
                }
            }
        }

        public void HidePanel()
        {
            _panelUI.SetActive(false);
        }

        public void UpgradeSelected(int selectedUpgradeIndex)
        {
            var movePanelToUpTween = MovePanelToUpAnimation();

            movePanelToUpTween
                .OnComplete(() =>
                {
                    OnUpgradeSelected.Invoke(selectedUpgradeIndex);
                });
        }

        private Tweener MovePanelToUpAnimation()
        {
            return _panelUI.transform
                .DOLocalMove(Vector3.up * 500f, _duration)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }
    }
}
