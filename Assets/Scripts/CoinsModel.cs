// Plain C# implementation of ICoinsModel - no Unity types, so it can be constructed without a
// scene. CoinsController owns the one instance and is the only thing that touches it.
public class CoinsModel : ICoinsModel
{
    public int CoinCount { get; private set; }

    public void Gain()
    {
        CoinCount++;
    }
}
