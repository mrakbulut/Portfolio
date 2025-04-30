using System.Collections.Generic;
using Portfolio.Collectible;
using Portfolio.Utility;

namespace Portfolio.Leveling
{
    public class ExperienceCollectibleCollector : ICollectibleCollector
    {
        private readonly PlayerLevel _playerLevel;
        private readonly List<SerializableGuid> _collectibleList;

        public ExperienceCollectibleCollector(PlayerLevel playerLevel, CollectibleType[] collectibleTypes)
        {
            _playerLevel = playerLevel;

            _collectibleList = new List<SerializableGuid>();
            for (int i = 0; i < collectibleTypes.Length; i++)
            {
                var collectibleId = collectibleTypes[i].Id;
                if (_collectibleList.Contains(collectibleId)) continue;

                _collectibleList.Add(collectibleId);
            }
        }

        public bool CanCollect(ICollectible collectible)
        {
            return _collectibleList.Contains(collectible.CollectibleTypeId);
        }

        public void Collect(ICollectible collectible)
        {
            if (!CanCollect(collectible)) return;

            if (collectible is IExperienceCollectible experienceCollectible)
            {
                int experience = experienceCollectible.Experience; // TODO: Multiply with Experience Rate
                _playerLevel.AddExperience(experience);
            }
        }
    }
}
