namespace Portfolio.Unit
{
    public class StatusEffectInstance
    {
        public StatusEffect Effect { get; private set; }
        public float? RemainingDuration { get; private set; }
        public IUnit Target { get; private set; }

        public StatusEffectInstance(StatusEffect effect, IUnit target)
        {
            Effect = effect;
            Target = target;
            RemainingDuration = effect.Data.IsPermanent ? null : effect.Duration;
        }

        public void UpdateDuration(float deltaTime)
        {
            if (RemainingDuration.HasValue)
            {
                RemainingDuration -= deltaTime;
            }
        }

        public bool IsExpired()
        {
            return RemainingDuration.HasValue && RemainingDuration <= 0;
        }
    }
}
