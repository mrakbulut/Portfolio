using Portfolio.Unit;
using UnityEngine;
using UnityEngine.Pool;

namespace Portfolio.Projectile
{
    public interface IProjectile
    {
        IUnit Source { get; }


        //void Update(float deltaTime);
        void Setup(IUnit source, Vector3 direction);
        void SetActive(bool active);
        void SetParent(Transform parent);
        void Initialize(IObjectPool<IProjectile> pool);
        void Refresh();
        void SetPosition(Vector3 position);
        void ReturnToPool();
    }
}
