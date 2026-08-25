using UnityEngine;

// Unlocks Mario's laser. Looks up LaserWeapon directly rather than IUseableWeapon - unqualified,
// that lookup returns whichever useable weapon Unity enumerates first, and there are two now.
public class LaserPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if(player != null)
        {
            LaserWeapon laserWeapon = player.GetComponentInChildren<LaserWeapon>();
            if(laserWeapon != null)
            {
                laserWeapon.Equip();
            }
        }
    }
}
