using System.Collections.Generic;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Portfolio.Collectible
{
    [System.Serializable]
    public class CollectiblePool
    {
        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private Transform _parent;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private GameObject _collectiblePrefab;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private CollectibleType _collectibleType;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _initialPoolSize = 20;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _maxPoolSize = 40;

        private SerializableGuid _collectibleTypeId;
        public SerializableGuid CollectibleTypeId => _collectibleTypeId;

        private IObjectPool<ICollectible> _pool;

        private readonly List<ICollectible> _activeCollectibles = new List<ICollectible>();
        public int ActiveCollectibleCount => _activeCollectibles.Count;

        public void InitializePool()
        {
            _collectibleTypeId = _collectibleType.Id;

            _pool = new ObjectPool<ICollectible>(
                CreateCollectible,
                OnCollectibleRetrieved,
                OnCollectibleReturned,
                OnCollectibleDestroyed,
                true,
                _initialPoolSize,
                _maxPoolSize);
        }

        private ICollectible CreateCollectible()
        {
            var collectibleGO = Object.Instantiate(_collectiblePrefab, _parent);
            var collectible = collectibleGO.GetComponent<ICollectible>();
            collectible.Initialize(_pool);
            return collectible;
        }

        private void OnCollectibleRetrieved(ICollectible collectible)
        {
            collectible.SetActive(true);
            _activeCollectibles.Add(collectible);
        }

        private void OnCollectibleReturned(ICollectible collectible)
        {
            collectible.SetActive(false);
            _activeCollectibles.Remove(collectible);
        }

        private void OnCollectibleDestroyed(ICollectible collectible)
        {

        }

        public ICollectible GetCollectible()
        {
            var collectible = _pool.Get();
            collectible.Initialize(_pool);
            //_collectible.SetPosition(position);
            return collectible;
        }

        public void UpdateActiveCollectibles(float deltaTime)
        {
            for (int i = 0; i < _activeCollectibles.Count; i++)
            {
                _activeCollectibles[i].Tick(deltaTime);
            }
        }

        public void ReturnAllCollectiblesToPool()
        {
            for (int i = _activeCollectibles.Count - 1; i >= 0; i--)
            {
                _activeCollectibles[i].ReturnToPool();
            }
        }
    }
}
