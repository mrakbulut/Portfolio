namespace Portfolio.Save
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}