namespace Portfolio.Stats
{
    [System.Serializable]
    public class SerializedStatModifier
    {
        public StatType StatType;
        public float Value;
        public StatModifierType ModifierType;
        public int Order;

        public StatModifier CreateModifier(object source)
        {
            return new StatModifier(Value, ModifierType, Order, source);
        }
    }
}
