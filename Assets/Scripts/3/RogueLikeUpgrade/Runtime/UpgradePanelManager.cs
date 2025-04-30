using Portfolio.Leveling;
using Portfolio.Upgradeables;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.RogueLikeUpgrade
{
    public class UpgradePanelManager : MonoBehaviour
    {
        [SerializeField] private UpgradePanelUIManager _upgradePanelUIManager;
        [SerializeField] private int _maxUpgradeCount = 3;

        private PlayerLevel _playerLevel;
        private UpgradeableManager _upgradeableManager;

        [ShowInInspector] [ReadOnly]
        private int _upgradePoint;
        [ShowInInspector] [ReadOnly]
        private bool _showingUpgrade;

        private IUpgradeable[] _currentUpgrades;

        private void Start()
        {
            _upgradePanelUIManager.OnUpgradeSelected += OnUpgradeSelected;

            _playerLevel = ServiceLocator.Instance.Get<PlayerLevel>();
            _playerLevel.OnLevelUp += OnPlayerLevelUp;

            _upgradeableManager = ServiceLocator.Instance.Get<UpgradeableManager>();
        }

        private void OnPlayerLevelUp()
        {
            _upgradePoint++;
            ShowUpgrade();
        }

        private void OnUpgradeSelected(int selectedUpgradeIndex) // TODO: Get Selected Upgrade ??
        {
            _upgradePoint--;
            _currentUpgrades[selectedUpgradeIndex].Upgrade();

            if (_upgradePoint > 0)
            {
                ShowRandomUpgradesOnPanel();
            }
            else
            {
                CompleteUpgrade();
            }
        }

        private void ShowUpgrade()
        {
            if (_showingUpgrade) return;

            Time.timeScale = 0f;
            _showingUpgrade = true;
            ShowRandomUpgradesOnPanel();
        }

        private void ShowRandomUpgradesOnPanel()
        {
            _currentUpgrades = _upgradeableManager.GetRandomUpgrades(_maxUpgradeCount);
            _upgradePanelUIManager.ShowPanel(_currentUpgrades);
        }

        private void CompleteUpgrade()
        {
            _currentUpgrades = null;
            _showingUpgrade = false;
            _upgradePanelUIManager.HidePanel();
            Time.timeScale = 1f;
        }

        private void OnDestroy()
        {
            if (_playerLevel != null)
            {
                _playerLevel.OnLevelUp -= OnPlayerLevelUp;
            }
        }
    }
}
