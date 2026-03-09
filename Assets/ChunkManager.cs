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

    public int GetNextPrice()
    {
        return basePrice * (int)Mathf.Pow(2, chunksUnlocked);
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