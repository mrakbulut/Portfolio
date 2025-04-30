namespace Portfolio.Upgradeables
{
    public interface IUpgradeable
    {
        bool Active { get; }
        int Level { get; }
        bool ReachedMaxLevel { get; }
        void Upgrade();
        UpgradeSummaryData GetUpgradeSummaryData();

        event System.Action<IUpgradeable> OnActivated;
        event System.Action<IUpgradeable> OnUpgraded;
    }
}
