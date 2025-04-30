using UnityEngine;

namespace Portfolio.Collectible
{
    public class AcceleratingMover
    {
        private readonly Transform _transform;
        private readonly float _initialSpeed;
        private readonly float _accelerationRate;
        private readonly float _maxSpeed;
        private readonly float _arrivalDistance;
        private readonly float _sqrArrivalDistance;
        private Transform _targetTransform;
        private float _currentSpeed;
        private bool _isMoving;

        public bool IsMoving => _isMoving;

        public System.Action OnTargetReached = () => { };

        public AcceleratingMover(Transform transform, float initialSpeed, float accelerationRate, float maxSpeed, float arrivalDistance)
        {
            _transform = transform;
            _initialSpeed = initialSpeed;
            _accelerationRate = accelerationRate;
            _maxSpeed = maxSpeed;
            _arrivalDistance = arrivalDistance;
            _sqrArrivalDistance = arrivalDistance * arrivalDistance;
            _isMoving = false;
        }

        ~AcceleratingMover()
        {
            OnTargetReached = () => { };
        }

        public void StartMovement(Transform target)
        {
            _targetTransform = target;
            _currentSpeed = _initialSpeed;
            _isMoving = true;
        }

        public void StopMovement()
        {
            _isMoving = false;
        }

        public void Update(float deltaTime)
        {
            if (!_isMoving || _targetTransform == null)
            {
                return;
            }
            var direction = (_targetTransform.position - _transform.position).normalized;

            _currentSpeed = Mathf.Min(_currentSpeed + _accelerationRate * deltaTime, _maxSpeed);

            _transform.position += direction * _currentSpeed * deltaTime;

            float dist = Vector3.Distance(_transform.position, _targetTransform.position);
            //float sqrDist = (_transform.position - _targetTransform.position).sqrMagnitude;
            if (dist < _arrivalDistance)
            {
                OnReachedToTarget();
            }
        }

        private void OnReachedToTarget()
        {
            _isMoving = false;
            _targetTransform = null;
            OnTargetReached.Invoke();
        }
    }
}
