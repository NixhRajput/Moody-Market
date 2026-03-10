using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager Instance;

    [Header("Base price for first chunk")]
    public int basePrice = 200;

    private int chunksUnlocked = 0;

    void Awake()
    {
        Instance = this;
    }

    private int[] chunkPrices = { 150, 400, 900, 1800 };

    public int GetNextPrice()
    {
        if (chunksUnlocked >= chunkPrices.Length) return chunkPrices[chunkPrices.Length - 1];
        return chunkPrices[chunksUnlocked];
    }

    public bool CanAfford()
    {
        return ShopManager.Instance.GetCoins() >= GetNextPrice();
    }

    public void UnlockChunk(GameObject border)
    {
        int price = GetNextPrice();
        ShopManager.Instance.SpendCoins(price);
        chunksUnlocked++;
        Debug.Log($"[Chunk] Unlocked! Spent {price} coins. Total unlocked: {chunksUnlocked}");
        Destroy(border);
    }
}