using UnityEngine;

namespace Portfolio.Commands
{
    public abstract class Command : MonoBehaviour
    {
        public System.Action<Command> onCommandComplete = _ => { };

        protected float percentage;
        public float Percentage => percentage;

        public abstract void StartCommand();

        public virtual void CompleteCommand()
        {
            percentage = 1f;

            onCommandComplete.Invoke(this);
            onCommandComplete = _ => { };
        }
        public abstract void ResetCommand();
    }
}
