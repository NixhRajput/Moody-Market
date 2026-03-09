using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    void Update()
    {
        if (ShopManager.Instance != null)
            coinsText.text = $"{ShopManager.Instance.GetCoins()} rs.";
    }
}