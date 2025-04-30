using Portfolio.Stats;
using UnityEngine;

namespace Portfolio.Damages
{
    [CreateAssetMenu(menuName = "Portfolio/Damage/Damage Type")]
    public class DamageType : ScriptableObject
    {
        [SerializeField] private StatType _damageStatType;
        [SerializeField] private StatType _resistanceStatType;

        public StatType DamageStatStatType => _damageStatType;
        public StatType ResistanceStatStatType => _resistanceStatType;
    }
}
