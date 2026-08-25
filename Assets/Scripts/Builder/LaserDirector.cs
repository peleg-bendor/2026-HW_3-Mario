using UnityEngine;

// Owns the one laser configuration this game has: the specific speed, lifetime and size that
// make something a laser, in the order they need to be set. A caller only ever asks for
// ConstructLaser() then Build() - never the builder's individual Set calls directly.
public class LaserDirector
{
    private readonly ILaserBuilder builder;

    public LaserDirector(ILaserBuilder builder)
    {
        this.builder = builder;
    }

    public void ConstructLaser()
    {
        builder.SetSpeed(5f);
        builder.SetLifetime(3f);
        builder.SetSize(1f);
    }

    public GameObject Build(GameObject prefab)
    {
        return builder.Build(prefab);
    }
}
