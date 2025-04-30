namespace Portfolio.Collectible
{
    public interface ICollectibleCollector
    {
        bool CanCollect(ICollectible collectible);
        void Collect(ICollectible collectible);
    }
}
