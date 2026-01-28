using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public static CoinUI Instance;

    public Text coinText;
    public Transform coinIconTransform;
    
    private int currentCoins = 0;
    private float displayedCoins = 0;
    public float lerpSpeed = 5f;

    void Awake()
    {
        Instance = this;
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
    }

    void Update()
    {
        if (displayedCoins < currentCoins)
        {
            displayedCoins = Mathf.Lerp(displayedCoins, currentCoins, Time.deltaTime * lerpSpeed);
            if (currentCoins - displayedCoins < 0.1f) displayedCoins = currentCoins;
            
            UpdateCoinText();
        }
        else if (displayedCoins > currentCoins)
        {
            displayedCoins = currentCoins;
            UpdateCoinText();
        }
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = Mathf.FloorToInt(displayedCoins).ToString();
        }
    }
}
