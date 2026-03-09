using UnityEngine;

public enum CustomerState { Waiting, PriceDropped, Gone }

public class Customer : MonoBehaviour
{
    public Sprite[] customerSprites;

    [HideInInspector] public string requestedCrop;
    [HideInInspector] public int requestedAmount;
    [HideInInspector] public int amountFulfilled;
    [HideInInspector] public float timeLeft;
    [HideInInspector] public float totalTime;
    [HideInInspector] public bool priceDropped = false;
    [HideInInspector] public CustomerState state = CustomerState.Waiting;

    private SpriteRenderer sr;
    private ShopConfig config;
    private bool active = false;

    public void Activate(ShopConfig cfg)
    {
        config = cfg;

        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        string[] crops = { "Wheat", "Tomato", "Carrot" };
        requestedCrop = crops[Random.Range(0, crops.Length)];
        requestedAmount = Random.Range(cfg.minAmount, cfg.maxAmount + 1);
        amountFulfilled = 0;
        timeLeft = Random.Range(cfg.minTime, cfg.maxTime);
        totalTime = timeLeft;
        priceDropped = false;
        state = CustomerState.Waiting;
        active = true;

        if (sr != null && customerSprites.Length > 0)
            sr.sprite = customerSprites[Random.Range(0, customerSprites.Length)];

        Debug.Log($"[Shop] Slot {gameObject.name} → wants {requestedAmount} {requestedCrop}");
    }

    public void CopyFrom(Customer other)
    {
        config = other.config;
        requestedCrop = other.requestedCrop;
        requestedAmount = other.requestedAmount;
        amountFulfilled = other.amountFulfilled;
        timeLeft = other.timeLeft;
        totalTime = other.totalTime;
        priceDropped = other.priceDropped;
        state = other.state;
        active = true;

        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        SpriteRenderer otherSr = other.GetComponentInChildren<SpriteRenderer>();
        if (sr != null && otherSr != null)
            sr.sprite = otherSr.sprite;
    }

    public void Deactivate()
    {
        active = false;
        state = CustomerState.Gone;
        timeLeft = 0f;
        requestedCrop = "";
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.sprite = null;
    }

    void Update()
    {
        if (!active || state == CustomerState.Gone) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            if (state == CustomerState.Waiting)
            {
                state = CustomerState.PriceDropped;
                priceDropped = true;
                timeLeft = totalTime * 0.4f;
                Debug.Log($"[Shop] Price dropped for {requestedCrop}!");
                ShopManager.Instance.RefreshUI();
            }
            else if (state == CustomerState.PriceDropped)
            {
                state = CustomerState.Gone;
                Debug.Log("[Shop] Customer left!");
                ShopManager.Instance.OnCustomerLeave(this);
            }
        }
    }

    public int GetCurrentPrice(ShopConfig cfg)
    {
        int base_ = cfg.GetPrice(requestedCrop);
        return priceDropped ? Mathf.RoundToInt(base_ * cfg.priceDropMultiplier) : base_;
    }

    public bool IsFullyFulfilled() => amountFulfilled >= requestedAmount;
}