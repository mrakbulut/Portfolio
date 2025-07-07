using UnityEngine;
namespace Boshphelm.Minimap
{
    public class CircularMover
    {
        private readonly float _radius;
        private readonly float _speed;
        private readonly Transform _transform;

        private float _currentAngle;
        private Vector3 _initialOffset;

        public CircularMover(Transform transform, float radius, float speed)
        {
            _transform = transform;
            _radius = radius;
            _speed = speed;

            SetInitialPosition();
        }

        private void SetInitialPosition()
        {
            _initialOffset = _transform.position;
            _currentAngle = CalculateInitialAngle();
        }

        private float CalculateInitialAngle()
        {
            return Mathf.Atan2(_initialOffset.z, _initialOffset.x) * Mathf.Rad2Deg;
        }

        public void MoveInCircle(float deltaTime)
        {
            float deltaAngle = _speed * deltaTime;

            _currentAngle += deltaAngle;

            if (_currentAngle > 360f)
            {
                _currentAngle -= 360f;
            }
            else if (_currentAngle < 0f)
            {
                _currentAngle += 360f;
            }

            var newPosition = CalculatePosition(_currentAngle);
            _transform.position = newPosition + _initialOffset;
        }

        private Vector3 CalculatePosition(float angle)
        {
            float radianAngle = angle * Mathf.Deg2Rad;
            var position = Vector3.zero;

            position.x += Mathf.Cos(radianAngle) * _radius;
            position.z += Mathf.Sin(radianAngle) * _radius;

            return position;
        }
    }
}
