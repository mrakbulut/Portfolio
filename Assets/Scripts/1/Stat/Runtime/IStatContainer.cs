using Portfolio.Utility;
namespace Portfolio.Stats
{
    public interface IStatContainer
    {
        Stat GetStatByStatTypeId(SerializableGuid statTypeId);
        Stat GetStatByStatType(StatType statType);
        void RemoveModifiersFromSource(object source);
    }
}
