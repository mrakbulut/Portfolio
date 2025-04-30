using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Skill
{
    public class UnitDetectorInArea
    {
        private readonly Transform _centerTransform;
        private readonly LayerMask _affectedLayers;
        private readonly GameObject _owner;
        private readonly bool _affectOwner;

        public UnitDetectorInArea(Transform centerTransform, LayerMask affectedLayers, GameObject owner, bool affectOwner)
        {
            _centerTransform = centerTransform;
            _affectedLayers = affectedLayers;
            _owner = owner;
            _affectOwner = affectOwner;
        }

        public HashSet<GameObject> DetectUnitsInArea(float area)
        {
            var detectedUnits = new HashSet<GameObject>();

            if (_centerTransform == null) return detectedUnits;

            var hitColliders = Physics.OverlapSphere(_centerTransform.position, area, _affectedLayers);

            foreach (var hitCollider in hitColliders)
            {
                var unit = hitCollider.gameObject;
                if (!_affectOwner && unit == _owner) continue;

                detectedUnits.Add(unit);
            }

            return detectedUnits;
        }
    }
}
