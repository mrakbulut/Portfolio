using Portfolio.Utility;

namespace Portfolio.Damages
{
    public class Damage
    {
        public readonly bool Resistible;
        public readonly float Value;
        public readonly SerializableGuid DamageStatType;
        public readonly SerializableGuid ResistanceStatType;
        // TODO: bool isSourcePlayer ?? SerializableGuid : sourceUnitId ??

        public Damage(bool resistible, float value, DamageType damageType)
        {
            Resistible = resistible;
            Value = value;
            DamageStatType = damageType.DamageStatStatType.Id;
            ResistanceStatType = damageType.ResistanceStatStatType.Id;
        }
    }
}
