using UnityEngine;

// Grants Mario one more boomerang. Looks up BoomerangWeapon directly rather than IReloadWeapon -
// unqualified, that lookup returns whichever reload weapon Unity enumerates first, and there are
// two now.
public class BoomerangPowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if(player != null)
        {
            BoomerangWeapon boomerangWeapon = player.GetComponentInChildren<BoomerangWeapon>();
            if(boomerangWeapon != null)
            {
                boomerangWeapon.Reload();
            }
        }
    }
}
