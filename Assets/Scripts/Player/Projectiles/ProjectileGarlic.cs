using UnityEngine;

// One garlic thrown by an enemy. Whether hitting Mario costs him health is not decided here -
// that comes from the SC_Death attached alongside this script.
public class ProjectileGarlic : BaseProjectile
{
    protected override bool TryHandleTarget(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            return false;

        GameLog.Info(LogCategory.Projectile, "Garlic hit Mario");
        Expire();
        return true;
    }

    protected override void OnTerrainHit()
    {
        GameLog.Verbose(LogCategory.Projectile, "Garlic hit a wall");
        Expire();
    }
}
