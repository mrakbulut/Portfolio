using UnityEngine;

namespace Portfolio.Leveling
{
    public class StandardLevelingFormula : ILevelingFormulaStrategy
    {
        private readonly float _baseExperience;
        private readonly float _multiplier;

        public StandardLevelingFormula(float baseExperience, float multiplier)
        {
            _baseExperience = baseExperience;
            _multiplier = multiplier;
        }

        public int CalculateRequiredExperience(int level)
        {
            return (int)(_baseExperience * Mathf.Pow(_multiplier, level));
        }
    }
}
