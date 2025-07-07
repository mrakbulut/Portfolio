using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Boshphelm.Minimap
{
    public class MinimapPlayerEntityPositionBridge
    {
        private readonly Transform _transform;
        private readonly Entity _entity;

        private EntityManager _entityManager;
        public bool HasEntity => _entity != Entity.Null;

        public MinimapPlayerEntityPositionBridge(Transform transform, Entity entity)
        {
            _transform = transform;
            _entity = entity;

            var world = World.DefaultGameObjectInjectionWorld;
            _entityManager = world.EntityManager;
        }

        public void Tick(float deltaTime)
        {
            if (!HasEntity) return;
            if (!_entityManager.Exists(_entity)) return;

            UpdateMinimapPosition();
        }

        private void UpdateMinimapPosition()
        {
            var localTransform = _entityManager.GetComponentData<LocalTransform>(_entity);

            localTransform.Position = _transform.position;
            localTransform.Rotation = _transform.rotation;

            _entityManager.SetComponentData(_entity, localTransform);
        }
    }
}
