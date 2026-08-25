using UnityEngine;

// Mario's boomerang: a stockpile he can throw from and refill by collecting more, whether from
// the level or by catching one back out of the air. Owns the count and announces every change,
// so nothing else has to track how many boomerangs he has.
public class BoomerangWeapon : MonoBehaviour, IReloadWeapon
{
    public static event System.Action<int> OnBoomerangCountChanged;

    public GameObject projectile;
    private int boomerangsHeld = 0;

    void Start()
    {
        // Announced once at startup so a display reading this count doesn't need its own copy
        // of the starting value.
        GameLog.Info(LogCategory.Weapon, "Starting with " + boomerangsHeld + " boomerang(s)");
        OnBoomerangCountChanged?.Invoke(boomerangsHeld);
    }

    public void Attack()
    {
        if (projectile != null && boomerangsHeld > 0)
        {
            GameObject curProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            ProjectileBoomerang scProjectile = curProjectile.GetComponent<ProjectileBoomerang>();
            if (scProjectile != null)
            {
                // Mario's facing lives on his sprite's scale, one level up from this weapon.
                float direction = 1;
                if (transform.parent != null)
                    direction = transform.parent.localScale.x;
                scProjectile.Fire(direction);
            }
            boomerangsHeld--;
            GameLog.Info(LogCategory.Weapon, "Boomerang thrown - " + boomerangsHeld + " left");
            OnBoomerangCountChanged?.Invoke(boomerangsHeld);
        }
        else
        {
            GameLog.Info(LogCategory.Weapon, "Boomerang attack ignored - no boomerangs held");
        }
    }

    // Called from both ways of gaining one: catching a returning boomerang and walking over a
    // fresh one in the level.
    public void Reload()
    {
        boomerangsHeld++;
        GameLog.Info(LogCategory.Weapon, "Boomerang gained - now holding " + boomerangsHeld);
        OnBoomerangCountChanged?.Invoke(boomerangsHeld);
    }

    // Always true, matching the axe: never locked, so weapon cycling can land on it even with
    // none currently held.
    public bool IsAvailable()
    {
        return true;
    }
}

