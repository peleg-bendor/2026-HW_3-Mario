using UnityEngine;

// Unlocks Mario's fireball. Looks up FireballWeapon directly rather than IUseableWeapon -
// unqualified, that lookup returns whichever useable weapon Unity enumerates first, and there
// are two now.
public class FireFlowerPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if(player != null)
        {
            FireballWeapon fireballWeapon = player.GetComponentInChildren<FireballWeapon>();
            if(fireballWeapon != null)
            {
                fireballWeapon.Equip();
            }
        }
    }
}
