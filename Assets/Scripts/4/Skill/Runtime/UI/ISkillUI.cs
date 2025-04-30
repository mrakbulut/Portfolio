namespace Portfolio.Skill
{
    public interface ISkillUI
    {
        void Initialize<T>(T skillBase, SkillData skillData) where T : SkillBase;
        void Show();
        void Hide();
        void Terminate();
    }
}
