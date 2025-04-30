using System.Collections.Generic;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Portfolio.Projectile
{
    [System.Serializable]
    public class ProjectilePool : IProjectilePool
    {
        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private Transform _parent;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private GameObject _projectilePrefab;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private ProjectileType _projectileType;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _initialPoolSize = 20;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _maxPoolSize = 40;

        private SerializableGuid _projectileTypeId;
        public SerializableGuid ProjetileTypeId => _projectileTypeId;

        private IObjectPool<IProjectile> _pool;

        private readonly List<IProjectile> _activeProjectiles = new List<IProjectile>();

        public void InitializePool()
        {
            _projectileTypeId = _projectileType.Id;

            _pool = new ObjectPool<IProjectile>(
                CreateProjectile,
                OnProjectileRetrieved,
                OnProjectileReturned,
                OnProjectileDestroyed,
                true,
                _initialPoolSize,
                _maxPoolSize);
        }

        private IProjectile CreateProjectile()
        {
            var projectileGO = Object.Instantiate(_projectilePrefab, _parent);
            var projectile = projectileGO.GetComponent<IProjectile>();
            projectile.Initialize(_pool);
            return projectile;
        }

        private void OnProjectileRetrieved(IProjectile projectile)
        {
            projectile.SetActive(true);
            _activeProjectiles.Add(projectile);
        }

        private void OnProjectileReturned(IProjectile projectile)
        {
            projectile.SetParent(_parent);
            projectile.SetActive(false);
            projectile.Refresh();
            _activeProjectiles.Remove(projectile);
        }

        private void OnProjectileDestroyed(IProjectile projectile)
        {

        }

        public IProjectile GetProjectile()
        {
            var projectile = _pool.Get();
            projectile.Initialize(_pool);
            //projectile.SetPosition(position);
            return projectile;
        }

        public void UpdateActiveProjectiles(float deltaTime)
        {
            for (int i = _activeProjectiles.Count - 1; i >= 0; i--)
            {
                //_activeProjectiles[i].Update(deltaTime);
            }
        }

        public void ReturnAllProjectiles()
        {
            for (int i = _activeProjectiles.Count - 1; i >= 0; i--)
            {
                _activeProjectiles[i].ReturnToPool();
            }
        }
    }
}
