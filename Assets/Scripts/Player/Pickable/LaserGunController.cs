using UnityEngine;

// The laser gun lying in the level. Same shape as every other pickup controller: detect Mario,
// hand over a power-up, disappear.
public class LaserGunController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameLog.Info(LogCategory.Pickup, "Laser gun collected");
            this.gameObject.SetActive(false);
            col.gameObject.GetComponent<PlayerPowerUp>().CollectPowerUp(new LaserPowerUp());
        }
    }
}
