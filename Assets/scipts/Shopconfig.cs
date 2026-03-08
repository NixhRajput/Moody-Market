using UnityEngine;

[CreateAssetMenu(fileName = "ShopConfig", menuName = "Shop/Config")]
public class ShopConfig : ScriptableObject
{
    [Header("Prices per 1 unit")]
    public int wheatPrice = 5;
    public int tomatoPrice = 8;
    public int carrotPrice = 10;

    [Header("Order Amount Range")]
    public int minAmount = 20;
    public int maxAmount = 30;

    [Header("Timer")]
    public float minTime = 100f;
    public float maxTime = 120f;

    [Header("Timeout Penalty")]
    [Range(0f, 1f)] public float priceDropMultiplier = 0.7f;

    public int GetPrice(string cropName)
    {
        return cropName switch
        {
            "Wheat" => wheatPrice,
            "Tomato" => tomatoPrice,
            "Carrot" => carrotPrice,
            _ => 0
        };
    }
}