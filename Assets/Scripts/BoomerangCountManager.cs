using TMPro;
using UnityEngine;

// Draws how many boomerangs Mario is holding. Owns no count of its own - it listens for
// BoomerangWeapon saying the number changed, so the weapon is the only place that number lives.
public class BoomerangCountManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boomerangCountText;

    private void OnEnable()
    {
        BoomerangWeapon.OnBoomerangCountChanged += OnBoomerangCountChanged;
    }

    private void OnDisable()
    {
        BoomerangWeapon.OnBoomerangCountChanged -= OnBoomerangCountChanged;
    }

    private void OnBoomerangCountChanged(int boomerangCount)
    {
        if (boomerangCountText != null)
            boomerangCountText.text = "Boomerangs: " + boomerangCount;
    }
}
