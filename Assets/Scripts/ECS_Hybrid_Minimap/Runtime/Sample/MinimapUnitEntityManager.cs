using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Portfolio.Minimap
{
    public class MinimapUnitEntityManager
    {
        private readonly MinimapObjectType _minimapObjectType;
        private readonly Entity _entity;
        private EntityManager _entityManager;

        public Entity Entity => _entity;

        public MinimapUnitEntityManager(MinimapObjectType minimapObjectType, Entity entity)
        {
            _minimapObjectType = minimapObjectType;

            var world = World.DefaultGameObjectInjectionWorld;
            _entityManager = world.EntityManager;
            _entity = entity;

            InitializeEntityComponents();
        }

        private void InitializeEntityComponents()
        {
            _entityManager.AddComponentData(_entity, new LocalTransform());
            _entityManager.AddComponentData(_entity, new MinimapEnemy
            {
                IconScaleRate = 1f
            });
            _entityManager.AddComponentData(_entity, new MinimapWorldPosition());

            _entityManager.AddComponentData(_entity, new MinimapEnemyTag());
            _entityManager.AddComponentData(_entity, new MinimapPosition());
            _entityManager.AddComponentData(_entity, new MinimapVisibility
            {
                IsVisible = false
            });
            _entityManager.SetComponentEnabled<MinimapVisibility>(_entity, false);
        }

        public void SetVisibilityOnMinimap(bool visible, Vector3 worldPosition)
        {
            var minimapVisibility = _entityManager.GetComponentData<MinimapVisibility>(_entity);
            minimapVisibility.IsVisible = visible;
            _entityManager.SetComponentData(_entity, minimapVisibility);

            _entityManager.SetComponentEnabled<MinimapVisibility>(_entity, visible);

            if (visible)
            {
                MinimapHybridManager.Instance.ShowEntityOnMinimap(_entity, _minimapObjectType, worldPosition);
            }
            else
            {
                MinimapHybridManager.Instance.RemoveEntityFromMinimap(_entity);
            }
        }
    }
}
