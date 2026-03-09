using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("Cloud 1 - Order Info")]
    public SpriteRenderer orderIconRenderer;
    public TextMeshPro orderAmountText;

    [Header("Cloud 2 - Timer")]
    public TextMeshPro timerText;

    [Header("Crop Icons")]
    public Sprite wheatIcon;
    public Sprite tomatoIcon;
    public Sprite carrotIcon;

    [Header("Coins (optional text)")]
    public TextMeshPro coinsText;

    public void UpdateUI(Customer customer, ShopConfig config, int coins)
    {
        if (orderIconRenderer != null)
            orderIconRenderer.sprite = GetIcon(customer.requestedCrop);

        if (orderAmountText != null)
            orderAmountText.text = $"{customer.amountFulfilled}/{customer.requestedAmount}";

        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(customer.timeLeft);
            timerText.text = customer.state == CustomerState.PriceDropped
                ? $"<color=red>{seconds}s</color>"
                : $"{seconds}s";
        }

        if (coinsText != null)
        {
            int remaining = customer.requestedAmount - customer.amountFulfilled;
            int priceEach = customer.GetCurrentPrice(config);
            int potential = remaining * priceEach;
            coinsText.text = $"{potential}";
        }
    }

    Sprite GetIcon(string cropName)
    {
        return cropName switch
        {
            "Wheat" => wheatIcon,
            "Tomato" => tomatoIcon,
            "Carrot" => carrotIcon,
            _ => null
        };
    }
}