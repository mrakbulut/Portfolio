using Portfolio.Utility;
using UnityEngine;
using UnityEngine.Pool;

namespace Portfolio.Collectible
{
    public interface ICollectible
    {
        SerializableGuid CollectibleTypeId { get; }
        void Initialize(IObjectPool<ICollectible> pool);
        void SetActive(bool active);
        void MoveToCollector(ICollectibleCollector collector, Transform collectorPoint);
        void ReturnToPool();
        void Tick(float deltaTime);
    }
}
