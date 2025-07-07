using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
namespace Portfolio.Minimap
{
    public class MinimapHybridManager : MonoBehaviour
    {
        [TitleGroup("References")]
        [SerializeField] private MinimapSettingsManager _minimapSettingsManager;

        [TitleGroup("References/UI")]
        [SerializeField] private RectTransform _minimapRect;
        [TitleGroup("References/UI")]
        [SerializeField] private Image _minimapBackgroundImage;

        [TitleGroup("Player Icon")]
        [SerializeField] private bool _rotatePlayerIcon = true;

        [TitleGroup("Icon Prefabs")]
        [SerializeField] private MinimapIconUIPool _minimapIconUIPool;
        [TitleGroup("Icon Prefabs")]
        [SerializeField] private Sprite[] _iconSprites; // Index matches MinimapTrackType

        [TitleGroup("References/Player")]
        [SerializeField] private MinimapIconUI _playerIconUI;

        [TitleGroup("Performance")]
        [SerializeField] private float _updateRateByFPS = 30f;

        [TitleGroup("Settings")]
        [SerializeField] private bool _autoInitialize = true;
        private readonly Dictionary<Entity, MinimapIconUI> _entityToIcon = new Dictionary<Entity, MinimapIconUI>();
        private bool _active;
        private World _ecsWorld;

        private EntityManager _entityManager;

        private bool _initialized;

        private Entity _playerEntity;
        private float _updateInterval;

        private float _updateTimer;
        public static MinimapHybridManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //InitializeSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (_autoInitialize)
            {
                Initialize();
                Activate();
            }
        }

        private void Update()
        {
            if (!_initialized) return;
            if (!_active) return;

            _updateTimer += Time.deltaTime;
            if (_updateTimer >= _updateInterval)
            {
                _updateTimer = 0f;
                UpdatePlayerIcon();
                UpdateEntityIcons();
            }
        }

        public void Initialize()
        {
            if (_initialized) return;

            _ecsWorld = World.DefaultGameObjectInjectionWorld;
            _entityManager = _ecsWorld.EntityManager;

            _updateInterval = 1f / _updateRateByFPS;
            _updateTimer = 0f;

            _minimapIconUIPool.Initialize();

            _minimapSettingsManager.Initialize();

            _initialized = true;
        }

        public void Activate()
        {
            _active = true;
            _minimapSettingsManager.SetActive(true);
        }
        public void Deactivate()
        {
            _active = false;
            _minimapSettingsManager.SetActive(false);
        }

        public void RemovePlayerEntity()
        {
            _playerEntity = Entity.Null;
        }

        public void SetPlayerEntity(Entity playerEntity)
        {
            _playerEntity = playerEntity;
            SetPlayerIcon();
        }

        private void SetPlayerIcon()
        {
            var playerSprite = GetMinimapObjectTypeSprite(MinimapObjectType.Player);
            _playerIconUI.Setup();
            _playerIconUI.SetSprite(playerSprite);
        }

        public void ShowEntityOnMinimap(Entity entity, MinimapObjectType minimapObjectType, Vector3 worldPosition)
        {
            if (_entityToIcon.ContainsKey(entity)) return;

            var minimapIconUI = _minimapIconUIPool.GetMinimapIconUI();

            var minimapObjectTypeSprite = GetMinimapObjectTypeSprite(minimapObjectType);
            minimapIconUI.SetSprite(minimapObjectTypeSprite);

            var minimapIconInitialPosition = _minimapSettingsManager.WorldToMinimapPosition(worldPosition);
            minimapIconUI.SetPosition(minimapIconInitialPosition);

            _entityToIcon.Add(entity, minimapIconUI);
        }

        public void RemoveEntityFromMinimap(Entity entity)
        {
            RemoveEntityIcon(entity);
        }

        public void ResetAndDeactivate()
        {
            RemoveAllEnemyEntitiesFromMinimap();
            Deactivate();
        }

        private void RemoveAllEnemyEntitiesFromMinimap()
        {
            foreach (var minimapIconUI in _entityToIcon.Values)
            {
                minimapIconUI.ReturnToPool();
            }

            _entityToIcon.Clear();
        }

        private void UpdatePlayerIcon()
        {
            if (_playerIconUI == null || _playerEntity == Entity.Null) return;

            var minimapPlayer = _entityManager.GetComponentData<MinimapPlayer>(_playerEntity);
            _playerIconUI.SetPosition(minimapPlayer.Position);

            if (_rotatePlayerIcon)
            {
                float targetRotation = minimapPlayer.Rotation;
                /*float currentRotation = minimapPlayer.PreviousRotation;
                float newRotation = Mathf.LerpAngle(currentRotation, targetRotation, 5f * Time.deltaTime);*/
                _playerIconUI.transform.rotation = Quaternion.Euler(0, 0, targetRotation);
            }
        }

        private void UpdateEntityIcons()
        {
            var query = _entityManager.CreateEntityQuery(
                typeof(MinimapVisibility),
                typeof(MinimapPosition)
                );

            var entities = query.ToEntityArray(Allocator.Temp);
            var positions = query.ToComponentDataArray<MinimapPosition>(Allocator.Temp);
            var tracked = query.ToComponentDataArray<MinimapVisibility>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];

                if (!tracked[i].IsVisible)
                {
                    RemoveEntityIcon(entity);
                    continue;
                }

                if (entity == _playerEntity)
                {
                    continue;
                }

                UpdateIcon(entity, positions[i]);
            }

            entities.Dispose();
            positions.Dispose();
            tracked.Dispose();
        }

        private void RemoveEntityIcon(Entity entity)
        {
            if (!_entityToIcon.TryGetValue(entity, out var minimapIconUI)) return;

            minimapIconUI.ReturnToPool();

            _entityToIcon.Remove(entity);
        }

        private void UpdateIcon(Entity entity, MinimapPosition minimapPosition)
        {
            if (!_entityToIcon.TryGetValue(entity, out var icon)) return;

            icon.SetPosition(minimapPosition.Position);
        }

        private Sprite GetMinimapObjectTypeSprite(MinimapObjectType objectType)
        {
            int objectTypeIndex = (int)objectType;

            return objectTypeIndex < _iconSprites.Length ? _iconSprites[objectTypeIndex] : null;
        }
    }
    public enum MinimapObjectType
    {
        Player = 0,
        Enemy = 1,
        EliteEnemy = 2,
        Boss = 3,
        CollectibleItem = 4,
        Objective = 5
    }
}
