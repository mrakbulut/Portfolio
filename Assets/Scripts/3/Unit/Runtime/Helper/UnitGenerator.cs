using Portfolio.Utility;
using UnityEngine;

namespace Portfolio.Unit
{
    public class UnitGenerator
    {
        private readonly SerializableGuid _unitTypeId;
        private readonly GameObject _unitPrefab;

        public SerializableGuid UnitTypeId => _unitTypeId;

        public UnitGenerator(SerializableGuid unitTypeId, GameObject unitPrefab)
        {
            _unitTypeId = unitTypeId;
            _unitPrefab = unitPrefab;
        }

        public GameObject GenerateUnit()
        {
            return Object.Instantiate(_unitPrefab);
        }

        public GameObject GenerateUnit(Transform parent)
        {
            return Object.Instantiate(_unitPrefab, parent);
        }

        public GameObject GenerateUnit(Vector3 position)
        {
            return Object.Instantiate(_unitPrefab, position, Quaternion.identity);
        }

        public GameObject GenerateUnit(Vector3 position, Quaternion rotation)
        {
            return Object.Instantiate(_unitPrefab, position, rotation);
        }
    }
}
