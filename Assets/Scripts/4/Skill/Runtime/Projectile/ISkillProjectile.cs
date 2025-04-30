using Portfolio.Projectile;
using Portfolio.Stats;
using Portfolio.Unit;
using UnityEngine;

namespace Portfolio.Skill
{
    public interface ISkillProjectile : IProjectile
    {
        void Setup(IUnit owner, IStatContainer ownerStatContainer, IStatContainer skillStatContainer, Vector3 direction, int skillLevel);
    }
}
