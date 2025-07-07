using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
namespace Boshphelm.Minimap
{
    public class SimpleMinimapUnit : MonoBehaviour
    {
        [SerializeField] private MinimapObjectType _unitMinimapObjectType;

        [TitleGroup("Movement Settings")]
        [SerializeField] private float _movementSpeed = 10f;
        [SerializeField] private float _radius = 3f;


        [TitleGroup("Random Position Props")]
        [SerializeField] private float _xRange = 10f;
        [SerializeField] private float _zRange = 10f;

        private CircularMover _circularMover;

        private Entity _entity;
        private MinimapEntityPositionBridge _minimapEntityPositionBridge;
        private MinimapUnitEntityManager _minimapUnitEntityManager;

        private void Awake()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = world.EntityManager;
            _entity = entityManager.CreateEntity();

            _minimapUnitEntityManager = new MinimapUnitEntityManager(_unitMinimapObjectType, _entity);
            _minimapEntityPositionBridge = new MinimapEntityPositionBridge(transform, _entity);
        }

        private void Start()
        {
            PutUnitAtRandomPosition(_xRange, _zRange);
            _circularMover = new CircularMover(transform, _radius, _movementSpeed);

            MinimapHybridManager.Instance.ShowEntityOnMinimap(_entity, _unitMinimapObjectType, transform.position);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            _circularMover.MoveInCircle(deltaTime);
            _minimapEntityPositionBridge.SetPosition(transform.position);
        }

        private void PutUnitAtRandomPosition(float xRange, float zRange)
        {
            var randomPosition = new Vector3(Random.Range(-xRange, xRange), 0f, Random.Range(-zRange, zRange));

            transform.position = randomPosition;
            _minimapUnitEntityManager.SetVisibilityOnMinimap(true, randomPosition);
        }
    }
}
