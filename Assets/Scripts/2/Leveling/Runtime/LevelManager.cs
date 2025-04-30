using System;

namespace Portfolio.Leveling
{
    public class LevelManager : ILevelManager
    {
        private readonly IExperienceManager _experienceManager;
        private readonly ILevelingFormulaStrategy _levelingFormula;

        public int CurrentLevel { get; private set; }
        public int ExperienceRequiredForNextLevel { get; private set; }

        public event Action<int> OnLevelChanged = _ => { };

        public LevelManager(IExperienceManager experienceManager, ILevelingFormulaStrategy levelingFormula, int initialLevel)
        {
            _levelingFormula = levelingFormula;
            _experienceManager = experienceManager;
            CurrentLevel = initialLevel;

            ExperienceRequiredForNextLevel = _levelingFormula.CalculateRequiredExperience(CurrentLevel);
        }

        public void GainExperience(int experience)
        {
            _experienceManager.AddExperience(experience);
        }

        public bool TryLevelUp()
        {
            if (_experienceManager.CurrentExperience >= ExperienceRequiredForNextLevel)
            {
                CurrentLevel++;

                ExperienceRequiredForNextLevel = _levelingFormula.CalculateRequiredExperience(CurrentLevel);

                _experienceManager.ResetExperience();

                OnLevelChanged.Invoke(CurrentLevel);
                return true;
            }

            return false;
        }
    }
}
