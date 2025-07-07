using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boshphelm.Minimap
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(MinimapWorldPositionUpdateSystem))]
    public partial struct MinimapPositionCalculationSystem : ISystem
    {
        private EntityQuery _minimapSettingsQuery;
        private EntityQuery _minimapEntitiesQuery;
        private EntityQuery _minimapPlayerQuery;

        public void OnCreate(ref SystemState state)
        {
            _minimapSettingsQuery = state.GetEntityQuery(
                ComponentType.ReadOnly<MinimapSettings>(),
                ComponentType.ReadOnly<MinimapSettingsTag>()
                );

            _minimapEntitiesQuery = state.GetEntityQuery(
                ComponentType.ReadWrite<MinimapPosition>(),
                ComponentType.ReadOnly<MinimapWorldPosition>(),
                ComponentType.ReadOnly<MinimapVisibility>()
                );

            _minimapPlayerQuery = state.GetEntityQuery(
                ComponentType.ReadWrite<MinimapPlayer>(),
                ComponentType.ReadOnly<MinimapPlayerTag>(),
                ComponentType.ReadOnly<MinimapWorldPosition>(),
                ComponentType.ReadOnly<MinimapWorldRotation>()
                );

            state.RequireForUpdate(_minimapSettingsQuery);
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!_minimapSettingsQuery.TryGetSingleton<MinimapSettings>(out var settings))
            {
                return;
            }

            if (!settings.IsActive)
            {
                return;
            }

            UpdatePlayerPosition(settings, ref state);

            UpdateEntityPositions(settings);
        }

        private void UpdatePlayerPosition(MinimapSettings settings, ref SystemState state)
        {
            if (_minimapPlayerQuery.IsEmpty)
            {
                return;
            }

            var playerEntity = _minimapPlayerQuery.GetSingletonEntity();
            var player = _minimapPlayerQuery.GetSingleton<MinimapPlayer>();
            var playerWorldPosition = _minimapPlayerQuery.GetSingleton<MinimapWorldPosition>();
            var playerWorldRotation = _minimapPlayerQuery.GetSingleton<MinimapWorldRotation>();

            var minimapPos = WorldToMinimapPosition(playerWorldPosition.WorldPosition, settings);

            player.PreviousPosition = player.Position;
            player.PreviousRotation = player.Rotation;

            player.Position = minimapPos;
            player.Rotation = playerWorldRotation.WorldRotation.y;

            player.HasMoved = !player.PreviousPosition.Equals(player.Position);
            // player.IsInBounds = IsPositionInBounds(minimapPos, settings);

            state.EntityManager.SetComponentData(playerEntity, player);
        }

        private static float2 WorldToMinimapPosition(float3 worldPosition, MinimapSettings settings)
        {
            var worldPos2D = new float2(worldPosition.x, worldPosition.z);
            var relativeToMapCenter = worldPos2D - settings.MapCenter;
            var minimapPos = relativeToMapCenter * settings.WorldToMinimapScale;
            // UnityEngine.Debug.Log($"WORLD POSITION 2D : {worldPos2D}, RELATIVE MAP CENTER : {relativeToMapCenter}, MINIMAP POS : {minimapPos}");
            return minimapPos + settings.MinimapCenter;
        }

        private void UpdateEntityPositions(MinimapSettings settings)
        {
            int entityCount = _minimapEntitiesQuery.CalculateEntityCount();
            if (entityCount == 0) return;

            var worldPositions = _minimapEntitiesQuery.ToComponentDataArray<MinimapWorldPosition>(Allocator.TempJob);
            var minimapPositions = _minimapEntitiesQuery.ToComponentDataArray<MinimapPosition>(Allocator.TempJob);
            var visibilities = _minimapEntitiesQuery.ToComponentDataArray<MinimapVisibility>(Allocator.TempJob);

            var job = new MinimapPositionCalculationJob
            {
                WorldPositions = worldPositions,
                MinimapPositions = minimapPositions,
                Visibilities = visibilities,
                Settings = settings
            };

            var jobHandle = job.Schedule(entityCount, 32);
            jobHandle.Complete();

            _minimapEntitiesQuery.CopyFromComponentDataArray(minimapPositions);

            worldPositions.Dispose();
            minimapPositions.Dispose();
            visibilities.Dispose();
        }
    }

    [BurstCompile]
    public struct MinimapPositionCalculationJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<MinimapWorldPosition> WorldPositions;
        [ReadOnly] public NativeArray<MinimapVisibility> Visibilities;
        public NativeArray<MinimapPosition> MinimapPositions;
        [ReadOnly] public MinimapSettings Settings;

        public void Execute(int index)
        {
            if (!Visibilities[index].IsVisible)
            {
                return;
            }

            var worldPos = WorldPositions[index];
            var minimapPos = MinimapPositions[index];

            minimapPos.PreviousPosition = minimapPos.Position;

            var worldPos2D = new float2(worldPos.WorldPosition.x, worldPos.WorldPosition.z);
            var relativeToMapCenter = worldPos2D - Settings.MapCenter;
            var newMinimapPos = relativeToMapCenter * Settings.WorldToMinimapScale;
            newMinimapPos += Settings.MinimapCenter;

            minimapPos.Position = newMinimapPos;

            minimapPos.HasMoved = !minimapPos.PreviousPosition.Equals(minimapPos.Position);

            //var halfSize = Settings.MinimapSize * 0.5f;
            //minimapPos.IsInBounds = math.abs(newMinimapPos.x) <= halfSize.x &&math.abs(newMinimapPos.y) <= halfSize.y;

            MinimapPositions[index] = minimapPos;
        }
    }
}
