using UnityEngine;

// Wires CoinsModel to CoinsView and to the coin pickup event. Subscribes to SC_Coin's collection
// event, applies the change to the model, and pushes the result to the view.
public class CoinsController : MonoBehaviour
{
    [SerializeField] private CoinsView coinsView;

    private ICoinsModel model;

    private void OnEnable()
    {
        SC_Coin.OnCoinCollision += OnCoinCollision;
    }

    private void OnDisable()
    {
        SC_Coin.OnCoinCollision -= OnCoinCollision;
    }

    private void Awake()
    {
        model = new CoinsModel();

        if (coinsView == null)
            GameLog.Warning(LogCategory.Pickup, "No CoinsView assigned, coin count will not be drawn");
    }

    private void Start()
    {
        if (coinsView != null)
            coinsView.ShowCoins(model.CoinCount);
    }

    private void OnCoinCollision()
    {
        model.Gain();

        GameLog.Info(LogCategory.Pickup, "Coin collected - " + model.CoinCount + " total");

        if (coinsView != null)
            coinsView.ShowCoins(model.CoinCount);
    }
}
