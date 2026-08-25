using TMPro;
using UnityEngine;

// Draws the coin count onto a label. Owns no count of its own - CoinsController tells it what
// to show.
public class CoinsView : MonoBehaviour, ICoinsView
{
    [SerializeField] private TextMeshProUGUI coinsText;

    public void ShowCoins(int coinCount)
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + coinCount;
    }
}
