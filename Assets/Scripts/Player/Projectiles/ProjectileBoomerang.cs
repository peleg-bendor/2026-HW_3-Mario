using UnityEngine;

// One boomerang in flight, from the throw until it's lost. Phases through enemies rather than
// stopping at them, and only a wall changes its course - flies on unaffected after a kill, turns
// around at the first wall it hits, and is lost at the second one or once its lifetime runs out.
// Stands alone rather than inheriting BaseProjectile: bouncing, fading and surviving a kill are
// steps the fireball, garlic and laser have no use for.
public class ProjectileBoomerang : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 6f;

    // How long before the end of its life the boomerang starts fading, the same warning
    // ProjectileAxe gives before it's lost for good.
    [SerializeField] private float warningDuration = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color baseColor;
    private float age = 0f;

    // Whether it has already turned around once. Catching only counts from here on - the same
    // guard ProjectileAxe.hasLanded gives the axe, since a boomerang spawns overlapping Mario and
    // would otherwise catch itself the instant it's thrown.
    private bool hasTurnedAround = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
            GameLog.Warning(LogCategory.Projectile, "No Rigidbody2D found, the boomerang will not fly");

        if (spriteRenderer == null)
            GameLog.Warning(LogCategory.Projectile, "No SpriteRenderer found, the boomerang will not fade out");
        else
            baseColor = spriteRenderer.color;
    }

    void Update()
    {
        age += Time.deltaTime;

        // Fading alpha rather than tinting the sprite: SpriteRenderer.color is a multiplicative
        // tint already sitting at white, so lightening it further does nothing visible.
        float warningStart = lifetime - warningDuration;
        if (spriteRenderer != null && age >= warningStart)
        {
            float fadeProgress = Mathf.Clamp01((age - warningStart) / warningDuration);
            float alpha = Mathf.Lerp(baseColor.a, 0f, fadeProgress);
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        }

        if (age >= lifetime)
            Lose();
    }

    public void Fire(float direction)
    {
        if (rb != null)
        {
            transform.localScale = new Vector3(direction, 1, 1);
            // Impulse, not Force: a one-shot push has to be applied as one, or only a single
            // physics step's worth of it ever becomes velocity.
            rb.AddForce(new Vector2(direction * speed, 0f), ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTurnedAround && other.gameObject.tag == "Player")
        {
            Catch(other.gameObject);
            return;
        }

        IEnemy enemy = other.GetComponent<IEnemy>();
        if (enemy != null)
        {
            enemy.Kill();
            return;
        }

        if (other.GetComponent<SC_Floor>() != null)
            OnWallHit();
    }

    private void OnWallHit()
    {
        if (!hasTurnedAround)
        {
            hasTurnedAround = true;
            rb.linearVelocity = -rb.linearVelocity;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            GameLog.Info(LogCategory.Projectile, "Boomerang turned around");
        }
        else
        {
            Lose();
        }
    }

    private void Catch(GameObject player)
    {
        GameLog.Info(LogCategory.Projectile, "Boomerang caught");
        PlayerPowerUp playerPowerUp = player.GetComponent<PlayerPowerUp>();
        if (playerPowerUp != null)
            playerPowerUp.CollectPowerUp(new BoomerangPowerUp());
        Destroy(gameObject);
    }

    private void Lose()
    {
        GameLog.Info(LogCategory.Projectile, "Boomerang lost");
        Destroy(gameObject);
    }

    // Fires a boomerang dropped straight into the scene, with no weapon wired up yet to do it for you.
    [ContextMenu("Test Fire")]
    private void TestFire()
    {
        Fire(1f);
    }
}
