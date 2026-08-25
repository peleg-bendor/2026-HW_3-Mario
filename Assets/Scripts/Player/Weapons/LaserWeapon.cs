using UnityEngine;

// Mario's laser: locked until the power-up, then fired by pulling from LaserPoolManager's pool
// instead of being instantiated fresh each shot.
public class LaserWeapon : MonoBehaviour, IUseableWeapon
{
    private bool isEquipped = false;

    public void Attack()
    {
        if (!isEquipped)
        {
            GameLog.Info(LogCategory.Weapon, "Laser attack ignored - not equipped");
            return;
        }

        if (LaserPoolManager.Instance == null)
        {
            GameLog.Warning(LogCategory.Weapon, "No LaserPoolManager found, the laser will not fire");
            return;
        }

        ProjectileLaser laser = LaserPoolManager.Instance.GetPooledLaser();
        if (laser == null)
        {
            GameLog.Info(LogCategory.Weapon, "Laser attack ignored - pool exhausted");
            return;
        }

        float direction = 1;
        if (transform.parent != null)
            direction = transform.parent.localScale.x;

        laser.transform.position = transform.position;
        laser.gameObject.SetActive(true);
        laser.Fire(direction);
        GameLog.Info(LogCategory.Weapon, "Laser shot");
    }

    public void Equip()
    {
        isEquipped = true;
    }

    public void UnEquip()
    {
        isEquipped = false;
    }

    public bool IsAvailable()
    {
        return isEquipped;
    }
}
