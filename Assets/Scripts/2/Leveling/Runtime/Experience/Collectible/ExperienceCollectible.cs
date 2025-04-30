using Portfolio.Collectible;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Portfolio.Leveling
{
    public class ExperienceCollectible : MonoBehaviour, IExperienceCollectible
    {
        [SerializeField] private CollectibleType _collectibleType;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        [TitleGroup("Mover Settings")]
        [SerializeField] private float _initialMoveSpeed = 2f;
        [TitleGroup("Mover Settings")]
        [SerializeField] private float _accelerationRate = 5f;
        [TitleGroup("Mover Settings")]
        [SerializeField] private float _maxMoveSpeed = 15f;
        [TitleGroup("Mover Settings")]
        [SerializeField] private float _collectDistance = 0.2f;

        public SerializableGuid CollectibleTypeId => _collectibleType.Id;
        public int Experience => _experience;

        private IObjectPool<ICollectible> _pool;
        private bool _released;
        private bool _collecting;
        private int _experience;
        private ExperienceCollectibleCollector _experienceCollectibleCollector;

        private ICollectibleCollector _collector;
        private AcceleratingMover _acceleratingMover;

        public void Initialize(IObjectPool<ICollectible> pool)
        {
            _pool = pool;

            _acceleratingMover = new AcceleratingMover(
                transform,
                _initialMoveSpeed,
                _accelerationRate,
                _maxMoveSpeed,
                _collectDistance
                );
            _acceleratingMover.OnTargetReached += OnReachedToCollector;
        }

        public void Setup(int experience, Vector3 position)
        {
            _experience = experience;
            _released = false;
            _collecting = false;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;

            transform.position = position;
            // TODO: Do Spawn Animation ??
        }

        public void Tick(float deltaTime)
        {
            if (_acceleratingMover == null) return;

            _acceleratingMover.Update(deltaTime);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void MoveToCollector(ICollectibleCollector collector, Transform collectorPoint)
        {
            if (_collecting) return;

            _collecting = true;
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
            _collector = collector;
            _acceleratingMover.StartMovement(collectorPoint);
        }

        private void OnReachedToCollector()
        {
            OnCollected();
        }

        private void OnCollected()
        {
            if (_collector != null)
            {
                _collector.Collect(this);
            }

            ReturnToPool();
        }
        public void ReturnToPool()
        {
            if (_released) return;

            if (_acceleratingMover != null)
            {
                _acceleratingMover.StopMovement();
            }

            _released = true;
            _pool.Release(this);
        }

        private void OnDestroy()
        {
            if (_acceleratingMover != null)
            {
                _acceleratingMover.OnTargetReached -= OnReachedToCollector;
            }
        }
    }
    public interface IExperienceCollectible : ICollectible
    {
        public int Experience { get; }
    }
}
