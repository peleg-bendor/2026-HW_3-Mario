// Draws Mario's current coin count. Knows nothing about pickups - only how to show a number.
public interface ICoinsView
{
    void ShowCoins(int coinCount);
}
