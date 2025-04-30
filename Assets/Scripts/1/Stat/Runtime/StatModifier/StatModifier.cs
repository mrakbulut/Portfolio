namespace Portfolio.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        public float Value;
        public StatModifierType StatModifierType;
        public int Order;
        public object Source; // TODO: Interface olarak kullan.

        public StatModifier(float value, StatModifierType statModifierType, int order, object source)
        {
            Value = value;
            StatModifierType = statModifierType;
            Order = order;
            Source = source;
        }

        public StatModifier(float value, StatModifierType statModifierType) : this(value, statModifierType, (int)statModifierType, null)
        {
        }

        public StatModifier(float value, StatModifierType statModifierType, int order) : this(value, statModifierType, order, null)
        {
        }

        public StatModifier(float value, StatModifierType statModifierType, object source) : this(value, statModifierType, (int)statModifierType, source)
        {
        }

        public StatModifier(StatModifier serializedModifier, object source) : this(serializedModifier.Value, serializedModifier.StatModifierType, serializedModifier.Order, source)
        {
        }
    }
}
