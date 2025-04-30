using System.Collections.Generic;
using Portfolio.Utility;

namespace Portfolio.Stats
{
    public class Stat
    {
        private readonly SerializableGuid _statTypeId;
        public SerializableGuid StatTypeId => _statTypeId;

        private float _baseValue;

        private float _cachedTotalValue;

        public float TotalValue
        {
            get
            {
                if (_isDirty)
                {
                    _cachedTotalValue = CalculateFinalValue();
                    OnTotalValueChanged.Invoke(_cachedTotalValue);
                    _isDirty = false;
                }

                return _cachedTotalValue;
            }
        }

        public System.Action<float> OnTotalValueChanged = _ => { };

        private bool _isDirty;

        private readonly List<StatModifier> _modifiers = new List<StatModifier>();

        public Stat(float baseValue, SerializableGuid statTypeId)
        {
            _baseValue = baseValue;
            _statTypeId = statTypeId;
            _modifiers.Clear();
            _isDirty = true;
        }

        public void UpdateBaseValue(float newBaseValue)
        {
            _baseValue = newBaseValue;
            _isDirty = true;
        }

        public void RemoveSourceModifiers(object source)
        {
            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                if (_modifiers[i].Source == source) RemoveModifier(_modifiers[i]);
            }
        }

        public void AddModifier(StatModifier modifier)
        {
            _isDirty = true;
            _modifiers.Add(modifier);
            _modifiers.Sort(CompareModifierOrder);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            bool removed = _modifiers.Remove(modifier);
            if (!removed) return;

            _isDirty = true;
            //BroadcastTotalValue();
        }

        private int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
            {
                return -1;
            }

            if (a.Order > b.Order)
            {
                return 1;
            }

            return 0; // a.order == b.order
        }

        private float CalculateFinalValue()
        {
            float finalValue = _baseValue;
            float percentageSum = 0;

            foreach (var statModifier in _modifiers)
            {
                var modType = statModifier.StatModifierType;
                if (modType == StatModifierType.Flat)
                {
                    finalValue += statModifier.Value;
                }
                else if (modType == StatModifierType.PercentageAdd)
                {
                    percentageSum += statModifier.Value;
                }
                else if (modType == StatModifierType.PercentageMultiplier)
                {
                    finalValue *= 1 + statModifier.Value;
                }
            }

            finalValue *= 1 + percentageSum / 100f;


            return finalValue;
        }
    }
}
