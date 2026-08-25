using UnityEngine;

// Shared shape for a Mario-fired or enemy-fired projectile: reset facing, launch, run a
// lifetime, and end on either a target hit or terrain. Fire() itself is the fixed sequence; a
// subclass only supplies the steps below that make it a fireball, a garlic or a laser.
public abstract class BaseProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
            GameLog.Warning(LogCategory.Projectile, "No Rigidbody2D found, the projectile will not fire");
    }

    public void Fire(float direction)
    {
        if (rb == null)
            return;

        // Clears any timer left over from a previous life, so a reused projectile can't have an
        // old Expire() land partway through its new one.
        CancelInvoke(nameof(Expire));

        transform.localScale = new Vector3(direction, 1, 1);
        // Impulse, not Force: a one-shot push has to be applied as one, or only a single
        // physics step's worth of it ever becomes velocity.
        rb.AddForce(GetLaunchImpulse(direction), ForceMode2D.Impulse);

        Invoke(nameof(Expire), lifetime);
    }

    // Sideways by facing, at this projectile's own speed. The laser overrides this to fire
    // straight up instead.
    protected virtual Vector2 GetLaunchImpulse(float direction)
    {
        return new Vector2(direction * speed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (TryHandleTarget(other))
            return;

        // An allowlist rather than "stop at anything unrecognised": SC_Floor is what marks a
        // tile as a tile, so coins, pickups and Mario himself are flown through untouched.
        if (other.GetComponent<SC_Floor>() != null)
            OnTerrainHit();
    }

    // Tests whether other is this projectile's target, and if so, acts on the hit and returns
    // true. The default is a weapon against enemies: kill and expire. The garlic overrides this,
    // since it isn't a weapon and its target is Mario, not an enemy.
    protected virtual bool TryHandleTarget(Collider2D other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        if (enemy == null)
            return false;

        enemy.Kill();
        Expire();
        return true;
    }

    // What happens at a wall differs enough per projectile - a message naming which one, or
    // nothing at all for something meant to pass through - that there's no shared default.
    protected abstract void OnTerrainHit();

    protected virtual void Expire()
    {
        Destroy(gameObject);
    }
}
