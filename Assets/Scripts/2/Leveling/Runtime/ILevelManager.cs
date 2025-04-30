namespace Portfolio.Leveling
{
    public interface ILevelManager
    {
        event System.Action<int> OnLevelChanged;
        int CurrentLevel { get; }
        int ExperienceRequiredForNextLevel { get; }
        void GainExperience(int experience);
        bool TryLevelUp();
    }
}
