namespace Portfolio.Leveling
{
    public interface IExperienceManager
    {
        event System.Action<int> OnExperienceChanged;
        int CurrentExperience { get; }
        void AddExperience(int amount);
        void ResetExperience();
    }
}
