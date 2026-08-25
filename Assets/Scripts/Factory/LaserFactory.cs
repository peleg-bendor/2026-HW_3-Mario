using UnityEngine;

// Assembles a ready-to-fire laser through the builder and director, so nothing calling
// CreateLaser() needs to know construction happens in steps at all.
public class LaserFactory
{
    private readonly LaserDirector director;

    public LaserFactory()
    {
        ILaserBuilder builder = new LaserBuilder();
        director = new LaserDirector(builder);
        director.ConstructLaser();
    }

    public ProjectileLaser CreateLaser(GameObject prefab)
    {
        GameObject curProjectile = director.Build(prefab);

        ProjectileLaser laser = curProjectile.GetComponent<ProjectileLaser>();
        if (laser == null)
            GameLog.Warning(LogCategory.Projectile, "No ProjectileLaser found, laser factory produced an incomplete object: " + prefab.name);

        return laser;
    }
}
