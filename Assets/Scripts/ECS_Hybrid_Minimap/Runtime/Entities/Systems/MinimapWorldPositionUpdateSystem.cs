using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Portfolio.Minimap
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(MinimapPositionCalculationSystem))]
    [BurstCompile]
    public partial struct MinimapWorldPositionUpdateSystem : ISystem
    {
        private EntityQuery _minimapEntitiesQuery;
        private EntityQuery _minimapPlayerQuery;

        public void OnCreate(ref SystemState state)
        {
            _minimapEntitiesQuery = state.GetEntityQuery(
                ComponentType.ReadOnly<LocalTransform>(),
                ComponentType.ReadWrite<MinimapWorldPosition>(),
                ComponentType.ReadOnly<MinimapVisibility>()
                );

            _minimapPlayerQuery = state.GetEntityQuery(
                ComponentType.ReadOnly<LocalTransform>(),
                ComponentType.ReadWrite<MinimapPlayer>(),
                ComponentType.ReadOnly<MinimapPlayerTag>()
                );
        }

        public void OnUpdate(ref SystemState state)
        {
            UpdatePlayerWorldPosition(ref state);

            UpdateEntityWorldPositions(ref state);
        }

        private void UpdatePlayerWorldPosition(ref SystemState state)
        {
            if (_minimapPlayerQuery.IsEmpty)
            {
                return;
            }

            var playerEntity = _minimapPlayerQuery.GetSingletonEntity();
            var transform = state.EntityManager.GetComponentData<LocalTransform>(playerEntity);
            var playerWorldPosition = state.EntityManager.GetComponentData<MinimapWorldPosition>(playerEntity);
            var playerWorldRotation = state.EntityManager.GetComponentData<MinimapWorldRotation>(playerEntity);

            playerWorldPosition.PreviousWorldPosition = playerWorldPosition.WorldPosition;
            playerWorldPosition.WorldPosition = transform.Position;

            playerWorldRotation.PerviousWorldRotation = playerWorldRotation.WorldRotation;
            playerWorldRotation.WorldRotation = transform.Rotation.value.xyz;

            state.EntityManager.SetComponentData(playerEntity, playerWorldPosition);
            state.EntityManager.SetComponentData(playerEntity, playerWorldRotation);
        }

        [BurstCompile]
        private void UpdateEntityWorldPositions(ref SystemState state)
        {
            int entityCount = _minimapEntitiesQuery.CalculateEntityCount();
            if (entityCount == 0) return;

            var transforms = _minimapEntitiesQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
            var worldPositions = _minimapEntitiesQuery.ToComponentDataArray<MinimapWorldPosition>(Allocator.TempJob);
            var visibilities = _minimapEntitiesQuery.ToComponentDataArray<MinimapVisibility>(Allocator.TempJob);

            var job = new MinimapWorldPositionUpdateJob
            {
                Transforms = transforms,
                WorldPositions = worldPositions,
                Visibilities = visibilities
            };

            var jobHandle = job.Schedule(entityCount, 5);
            jobHandle.Complete();

            _minimapEntitiesQuery.CopyFromComponentDataArray(worldPositions);

            transforms.Dispose();
            worldPositions.Dispose();
            visibilities.Dispose();
        }
    }

    [BurstCompile]
    public struct MinimapWorldPositionUpdateJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<LocalTransform> Transforms;
        [ReadOnly] public NativeArray<MinimapVisibility> Visibilities;
        public NativeArray<MinimapWorldPosition> WorldPositions;

        public void Execute(int index)
        {
            if (!Visibilities[index].IsVisible)
            {
                return;
            }

            var transform = Transforms[index];
            var worldPos = WorldPositions[index];

            worldPos.PreviousWorldPosition = worldPos.WorldPosition;

            worldPos.WorldPosition = transform.Position;

            //worldPos.HasMoved = !worldPos.PreviousWorldPosition.Equals(worldPos.WorldPosition);

            WorldPositions[index] = worldPos;
        }
    }
}
