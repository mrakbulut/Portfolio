using Portfolio.Commands;

namespace Portfolio.InitialLoad
{
    public abstract class LoadCommand : Command
    {
        public abstract float PercentageComplete { get; }
    }
}
