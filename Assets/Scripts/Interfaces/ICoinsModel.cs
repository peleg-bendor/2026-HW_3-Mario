// Owns Mario's coin count and the rule for changing it. No Unity types - CoinsController wraps
// this and does the Unity-facing wiring.
public interface ICoinsModel
{
    int CoinCount { get; }

    void Gain();
}
