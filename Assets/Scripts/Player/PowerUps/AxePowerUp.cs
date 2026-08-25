using UnityEngine;

// Grants Mario one more axe. Looks up AxeWeapon directly rather than IReloadWeapon - unqualified,
// that lookup returns whichever reload weapon Unity enumerates first, and there are two now.
public class AxePowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if(player != null)
        {
            AxeWeapon axeWeapon = player.GetComponentInChildren<AxeWeapon>();
            if(axeWeapon != null)
            {
                axeWeapon.Reload();
            }
        }
    }
}
