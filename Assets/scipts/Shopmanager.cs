using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Config")]
    public ShopConfig config;

    [Header("Queue Slots (buyer, next, next_to_next in order)")]
    public Customer[] queueSlots;

    [Header("Sell Zone")]
    public Transform sellPlace;
    public float sellRange = 1.5f;

    [Header("Player")]
    public Transform player;
    public InventorySystem inventory;

    [Header("Shop UI")]
    public ShopUI shopUI;

    private int coins = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < queueSlots.Length; i++)
            queueSlots[i].Activate(config);

        RefreshUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TrySell();

        RefreshUI();
    }

    void TrySell()
    {
        if (Vector2.Distance(player.position, sellPlace.position) > sellRange)
        {
            Debug.Log("[Shop] Too far from sell spot!");
            return;
        }

        Customer front = queueSlots[0];
        if (front == null || front.state == CustomerState.Gone)
        {
            Debug.Log("[Shop] No active customer!");
            return;
        }

        string crop = front.requestedCrop;
        int inInventory = inventory.GetItemCount(crop);

        if (inInventory <= 0)
        {
            Debug.Log($"[Shop] No {crop} in inventory!");
            return;
        }

        int stillNeeded = front.requestedAmount - front.amountFulfilled;
        int toSell = Mathf.Min(inInventory, stillNeeded);
        int priceEach = front.GetCurrentPrice(config);

        inventory.RemoveItem(crop, toSell);
        front.amountFulfilled += toSell;

        int earned = toSell * priceEach;
        coins += earned;
        Debug.Log($"[Shop] Sold {toSell} {crop} for {earned} coins! ({front.amountFulfilled}/{front.requestedAmount}) Total: {coins}");
        if (AudioManager.Instance != null) AudioManager.Instance.PlayGain();

        if (front.IsFullyFulfilled())
        {
            Debug.Log("[Shop] Order complete!");
            AdvanceQueue();
        }

        RefreshUI();
    }

    public void OnCustomerLeave(Customer c)
    {
        AdvanceQueue();
        RefreshUI();
    }

    void AdvanceQueue()
    {
        queueSlots[0].Deactivate();

        for (int i = 0; i < queueSlots.Length - 1; i++)
            queueSlots[i].Activate(config);

        queueSlots[queueSlots.Length - 1].Activate(config);
        Debug.Log("[Shop] Queue advanced, fresh customers.");
    }

    public void RefreshUI()
    {
        if (shopUI == null) return;
        Customer front = queueSlots[0];
        if (front != null && front.state != CustomerState.Gone)
            shopUI.UpdateUI(front, config, coins);
    }

    public int GetCoins() => coins;

    public void SpendCoins(int amount)
    {
        coins -= amount;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayGain();
        Debug.Log($"[Shop] Spent {amount} coins. Remaining: {coins}");
    }

    void OnDrawGizmosSelected()
    {
        if (sellPlace != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sellPlace.position, sellRange);
        }
    }
}