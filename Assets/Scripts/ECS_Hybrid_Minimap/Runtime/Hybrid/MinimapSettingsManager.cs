using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace Portfolio.Minimap
{
    public class MinimapSettingsManager : MonoBehaviour
    {
        [TitleGroup("Map Settings")]
        [SerializeField] private Vector2 _mapCenter = Vector2.zero;
        [TitleGroup("Map Settings")]
        [SerializeField] private Vector2 _minimapCenter = Vector2.zero;
        [TitleGroup("Map Settings")]
        [SerializeField] private float _worldToMinimapScale = 0.1f;
        [TitleGroup("Map Settings")]
        [SerializeField] private Vector2 _worldBounds = new Vector2(100f, 100f);
        [TitleGroup("Map Settings")]
        [SerializeField] private Vector2 _minimapSize = new Vector2(200f, 200f);

        [TitleGroup("References")]
        [SerializeField] private RectTransform _minimapRect;

        [TitleGroup("Runtime")]
        [ReadOnly] [SerializeField] private bool _isActive;

        private EntityManager _entityManager;
        private World _ecsWorld;
        private Entity _settingsEntity;
        private bool _initialized;

        public void Initialize()
        {
            if (_initialized) return;

            InitializeECS();
            CreateSettingsEntity();
            UpdateMinimapSizeFromRect();
            _initialized = true;
        }

        private void InitializeECS()
        {
            _ecsWorld = World.DefaultGameObjectInjectionWorld;
            _entityManager = _ecsWorld.EntityManager;
        }

        private void CreateSettingsEntity()
        {
            _settingsEntity = _entityManager.CreateEntity();

            _entityManager.AddComponentData(_settingsEntity, new MinimapSettingsTag());
            _entityManager.AddComponentData(_settingsEntity, new MinimapSettings
            {
                IsActive = false,
                MapCenter = _mapCenter,
                MinimapCenter = _minimapCenter,
                WorldToMinimapScale = _worldToMinimapScale,
                WorldBounds = _worldBounds,
                MinimapSize = _minimapSize
            });

            UpdateSettingsEntity();
        }

        private void UpdateSettingsEntity()
        {
            if (_settingsEntity == Entity.Null) return;

            var settings = new MinimapSettings
            {
                MapCenter = _mapCenter,
                MinimapCenter = _minimapCenter,
                WorldToMinimapScale = _worldToMinimapScale,
                WorldBounds = _worldBounds,
                MinimapSize = _minimapSize,
                IsActive = _isActive
            };

            _entityManager.SetComponentData(_settingsEntity, settings);
        }

        private void UpdateMinimapSizeFromRect()
        {
            if (_minimapRect != null)
            {
                var rect = _minimapRect.rect;
                _minimapSize = new Vector2(rect.width, rect.height);
            }
        }

        public void SetActive(bool active)
        {
            _isActive = active;
            UpdateSettingsEntity();
        }

        public void SetMapCenter(Vector2 mapCenter)
        {
            _mapCenter = mapCenter;
            UpdateSettingsEntity();
        }

        public void SetWorldToMinimapScale(float scale)
        {
            _worldToMinimapScale = scale;
            UpdateSettingsEntity();
        }

        public void SetWorldBounds(Vector2 bounds)
        {
            _worldBounds = bounds;
            UpdateSettingsEntity();
        }

        public void SetMinimapCenter(Vector2 center)
        {
            _minimapCenter = center;
            UpdateSettingsEntity();
        }

        public Vector2 WorldToMinimapPosition(Vector3 worldPosition)
        {
            var worldPos2D = new Vector2(worldPosition.x, worldPosition.z);
            var relativeToMapCenter = worldPos2D - _mapCenter;
            var minimapPos = relativeToMapCenter * _worldToMinimapScale;
            return minimapPos + _minimapCenter;
        }
    }
}
