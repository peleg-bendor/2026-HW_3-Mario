using UnityEngine;

// One fireball in flight. Kills the first enemy it passes through and stops at the first wall,
// then destroys itself either way. The launch, the lifetime and the target-or-wall split all
// live in BaseProjectile; this class only supplies what makes a fireball a fireball.
public class ProjectileFireball : BaseProjectile
{
    protected override void OnTerrainHit()
    {
        GameLog.Verbose(LogCategory.Projectile, "Fireball hit a wall");
        Expire();
    }
}
