using System;
using Portfolio.Utility;

namespace Portfolio.Leveling
{
    public class BasicExperienceManager : IExperienceManager
    {
        public event Action<int> OnExperienceChanged = _ => { };
        public int CurrentExperience { get; private set; }

        public BasicExperienceManager(int initialExperience)
        {
            ServiceLocator.Instance.Register(this);
            CurrentExperience = initialExperience;
        }

        ~BasicExperienceManager()
        {
            OnExperienceChanged = _ => { };
        }

        public void AddExperience(int amount)
        {
            CurrentExperience += amount;

            OnExperienceChanged.Invoke(CurrentExperience);
        }

        public void ResetExperience()
        {
            CurrentExperience = 0;

            OnExperienceChanged.Invoke(CurrentExperience);
        }
    }
}
