using UnityEngine;

// Accumulates a laser's stats across Set calls, then stamps them onto a fresh instance of
// whatever prefab Build() is given. This class only knows how to apply speed, lifetime and size
// - deciding what they should be is LaserDirector's job, not this one's.
public class LaserBuilder : ILaserBuilder
{
    private float speed;
    private float lifetime;
    private float size;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }

    public GameObject Build(GameObject prefab)
    {
        GameObject curProjectile = Object.Instantiate(prefab);

        BaseProjectile projectile = curProjectile.GetComponent<BaseProjectile>();
        if (projectile == null)
        {
            GameLog.Warning(LogCategory.Projectile, "No BaseProjectile found, laser stats not applied: " + prefab.name);
            return curProjectile;
        }

        projectile.speed = speed;
        projectile.lifetime = lifetime;
        projectile.size = size;
        return curProjectile;
    }
}
