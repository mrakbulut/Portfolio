using Unity.Entities;
using Unity.Mathematics;

namespace Boshphelm.Minimap
{
    public struct MinimapSettings : IComponentData
    {
        public float2 MapCenter;
        public float2 MinimapCenter;
        public float WorldToMinimapScale;
        public float2 WorldBounds;
        public float2 MinimapSize;
        public bool IsActive;
    }

    public struct MinimapSettingsTag : IComponentData
    {
    }

    public struct MinimapVisibility : IComponentData, IEnableableComponent
    {
        public bool IsVisible;
    }

    public struct MinimapWorldPosition : IComponentData
    {
        public float3 WorldPosition;
        public float3 PreviousWorldPosition;
    }

    public struct MinimapWorldRotation : IComponentData
    {
        public float3 WorldRotation;
        public float3 PerviousWorldRotation;
    }

    public struct MinimapPosition : IComponentData
    {
        public float2 Position;
        public float2 PreviousPosition;
        public bool HasMoved;
    }

    public struct MinimapPlayer : IComponentData
    {
        public float2 Position;
        public float2 PreviousPosition;
        public float Rotation;
        public float PreviousRotation;
        public bool HasMoved;
    }

    public struct MinimapEnemy : IComponentData
    {
        public float IconScaleRate;
    }

    public struct MinimapCollectible : IComponentData
    {
        public bool IsCollected;
    }

    public struct MinimapEnemyTag : IComponentData
    {
    }
    public struct MinimapPlayerTag : IComponentData
    {
    }
    public struct MinimapCollectibleTag : IComponentData
    {
    }
}
