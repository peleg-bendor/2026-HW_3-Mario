using UnityEngine;

// One laser bolt in flight, fired straight up. Passes through tiles and pickups instead of
// stopping at them, and otherwise ends the same way the fireball does: killing the first enemy
// it touches, or running out its lifetime.
public class ProjectileLaser : BaseProjectile
{
    protected override Vector2 GetLaunchImpulse(float direction)
    {
        return new Vector2(0f, speed);
    }

    // Empty rather than absent: BaseProjectile's OnTriggerEnter2D still checks for SC_Floor on
    // every trigger, and an empty step is what lets the laser pass through it untouched.
    protected override void OnTerrainHit()
    {
    }

    // Fires a laser dropped straight into the scene, with no weapon wired up yet to do it for you.
    [ContextMenu("Test Fire")]
    private void TestFire()
    {
        Fire(1f);
    }
}
