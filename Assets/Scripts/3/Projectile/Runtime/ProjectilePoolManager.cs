using System.Collections.Generic;
using Portfolio.Utility;
using UnityEngine;
namespace Portfolio.Projectile
{
    public class ProjectilePoolManager : MonoBehaviour
    {
        [SerializeField] private List<ProjectilePool> _projectilePools;

        private Dictionary<SerializableGuid, IProjectilePool> _poolsByType;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            InitializePools();
        }
        private void InitializePools()
        {
            _poolsByType = new Dictionary<SerializableGuid, IProjectilePool>();

            foreach (var projectilePool in _projectilePools)
            {
                projectilePool.InitializePool();
                _poolsByType.Add(projectilePool.ProjetileTypeId, projectilePool);
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (var projectilePool in _projectilePools)
            {
                projectilePool.UpdateActiveProjectiles(deltaTime);
            }
        }

        public IProjectilePool GetProjectilePoolByProjectileTypeId(SerializableGuid projectileTypeId)
        {
            if (_poolsByType.TryGetValue(projectileTypeId, out var pool))
            {
                return pool;
            }

            Debug.LogError("Projectile Pool Not Found :  " + projectileTypeId.ToHexString());
            return null;
        }

        public void ReturnAllActiveProjectilesToPool()
        {
            for (int i = 0; i < _projectilePools.Count; i++)
            {
                _projectilePools[i].ReturnAllProjectiles();
            }
        }
    }
}
