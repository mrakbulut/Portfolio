using System.Collections.Generic;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Portfolio.Collectible
{
    public class CollectiblePoolManager : MonoBehaviour
    {
        [SerializeField] private List<CollectiblePool> _collectiblePools;

        [Title("Debug")]
        [SerializeField] private TextMeshProUGUI _activeCollectibleCountText;

        private Dictionary<SerializableGuid, CollectiblePool> _poolsByType;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            InitializePools();
        }
        private void InitializePools()
        {
            _poolsByType = new Dictionary<SerializableGuid, CollectiblePool>();

            foreach (var collectiblePool in _collectiblePools)
            {
                collectiblePool.InitializePool();
                _poolsByType.Add(collectiblePool.CollectibleTypeId, collectiblePool);
            }
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        public void Tick(float deltaTime)
        {
            int totalActiveCollectibleCount = 0;
            foreach (var collectiblePool in _collectiblePools)
            {
                collectiblePool.UpdateActiveCollectibles(deltaTime);
                totalActiveCollectibleCount += collectiblePool.ActiveCollectibleCount;
            }

            _activeCollectibleCountText.text = "COLLECTIBLE COUNT : " + totalActiveCollectibleCount;
        }

        public CollectiblePool GetCollectiblePoolByCollectibleTypeId(SerializableGuid collectibleTypeId)
        {
            if (_poolsByType.TryGetValue(collectibleTypeId, out var pool))
            {
                return pool;
            }

            Debug.LogError("Collectible Pool Not Found :  " + collectibleTypeId.ToHexString());
            return null;
        }

        public void CollectiblesReturnToPool()
        {
            for (int i = 0; i < _collectiblePools.Count; i++)
            {
                _collectiblePools[i].ReturnAllCollectiblesToPool();
            }
        }
    }
}
