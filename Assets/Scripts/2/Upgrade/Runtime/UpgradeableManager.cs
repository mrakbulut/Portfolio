using System.Collections.Generic;
using Portfolio.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Portfolio.Upgradeables
{
    public class UpgradeableManager : MonoBehaviour
    {
        private readonly List<IUpgradeable> _upgradeables = new List<IUpgradeable>();
        private readonly List<IUpgradeable> _availableUpgradeables = new List<IUpgradeable>();
        private readonly List<IUpgradeable> _maxLevelUpgradeables = new List<IUpgradeable>();

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
        }

        public void AddUpgrade(IUpgradeable upgradeable)
        {
            if (upgradeable == null) return;

            _upgradeables.Add(upgradeable);
            if (upgradeable.ReachedMaxLevel)
            {
                _maxLevelUpgradeables.Add(upgradeable);
            }
            else
            {
                _availableUpgradeables.Add(upgradeable);
            }

            upgradeable.OnUpgraded += OnUpgradeableUpgraded;
        }

        public void RemoveUpgrade(IUpgradeable upgradeable)
        {
            if (upgradeable == null) return;

            _upgradeables.Remove(upgradeable);
            _availableUpgradeables.Remove(upgradeable);
            _maxLevelUpgradeables.Remove(upgradeable);

            upgradeable.OnUpgraded -= OnUpgradeableUpgraded;
        }

        public IUpgradeable[] GetRandomUpgrades(int maxCount) // TODO: Add Upgrade Type Constraint ??
        {
            var randomUpgrades = new List<IUpgradeable>();
            var availableUpgradesMirror = new List<IUpgradeable>(_availableUpgradeables);

            int targetUpgradeCount = availableUpgradesMirror.Count >= maxCount ? maxCount : availableUpgradesMirror.Count;

            while (randomUpgrades.Count < targetUpgradeCount)
            {
                int randomIndex = Random.Range(0, availableUpgradesMirror.Count);

                var selectedUpgrade = availableUpgradesMirror[randomIndex];
                randomUpgrades.Add(selectedUpgrade);
                availableUpgradesMirror.RemoveAt(randomIndex);
            }

            return randomUpgrades.ToArray();
        }

        public void OnUpgradeableUpgraded(IUpgradeable upgradeable)
        {
            //Debug.Log("REACHED AT MAX LEVEL : " + upgradeable.ReachedMaxLevel);
            if (upgradeable.ReachedMaxLevel)
            {
                if (!_maxLevelUpgradeables.Contains(upgradeable))
                {
                    _maxLevelUpgradeables.Add(upgradeable);
                }
                _availableUpgradeables.Remove(upgradeable);
                //Debug.Log("AVAILABLE UPGRADEABLES COUNT : " + _availableUpgradeables.Count);
            }
            else
            {
                _maxLevelUpgradeables.Remove(upgradeable);
                if (!_availableUpgradeables.Contains(upgradeable))
                {
                    _availableUpgradeables.Add(upgradeable);
                }
            }
        }

        private void OnDestroy()
        {
            for (int i = _availableUpgradeables.Count - 1; i >= 0; i--)
            {
                _availableUpgradeables[i].OnUpgraded -= OnUpgradeableUpgraded;
                _availableUpgradeables.RemoveAt(i);
            }

            for (int i = _maxLevelUpgradeables.Count - 1; i >= 0; i--)
            {
                _maxLevelUpgradeables[i].OnUpgraded -= OnUpgradeableUpgraded;
                _maxLevelUpgradeables.RemoveAt(i);
            }

            for (int i = _upgradeables.Count - 1; i >= 0; i--)
            {
                _upgradeables[i].OnUpgraded -= OnUpgradeableUpgraded;
                _upgradeables.RemoveAt(i);
            }
        }
    }
}
