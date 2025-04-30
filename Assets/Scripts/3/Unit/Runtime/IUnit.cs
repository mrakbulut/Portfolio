using Portfolio.Damages;
using Portfolio.Stats;
using Portfolio.Utility;

namespace Portfolio.Unit
{
    public interface IUnit
    {
        //long ID { get; }
        SerializableGuid UnitTypeId { get; }
        SerializableGuid TeamTypeId { get; }
        Health Health { get; }
        IStatContainer StatContainer { get; }
        StatusEffectManager StatusEffectManager { get; }

        void TakeDamage(Damage damage);
    }
}
