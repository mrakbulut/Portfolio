using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Boshphelm.Minimap
{
    public class MinimapEntityPositionBridge
    {
        private readonly Transform _transform;
        private readonly Entity _entity;

        private EntityManager _entityManager;

        public bool HasEntity => _entity != Entity.Null;

        public MinimapEntityPositionBridge(Transform transform, Entity entity)
        {
            _transform = transform;
            _entity = entity;

            var world = World.DefaultGameObjectInjectionWorld;
            _entityManager = world.EntityManager;
        }

        public void SetPosition(Vector3 position)
        {
            UpdateMinimapPosition(position);
        }

        public void Tick(float deltaTime)
        {
            if (!HasEntity) return;
            if (!_entityManager.Exists(_entity)) return;

            UpdateMinimapPosition(_transform.position);
        }

        private void UpdateMinimapPosition(Vector3 position)
        {
            var localTransform = _entityManager.GetComponentData<LocalTransform>(_entity);

            localTransform.Position = position;

            _entityManager.SetComponentData(_entity, localTransform);
        }
    }
}
