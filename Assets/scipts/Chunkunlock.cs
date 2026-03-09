using UnityEngine;
using TMPro;

public class ChunkUnlock : MonoBehaviour
{
    [Header("The border parent to destroy on unlock")]
    public GameObject borderObject;

    public float interactRange = 1f;

    [Header("Price popup (TextMeshPro on a child object)")]
    public TextMeshPro pricePopup;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (borderObject == null)
            borderObject = transform.parent.parent.gameObject;

        if (pricePopup != null)
            pricePopup.gameObject.SetActive(false);
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= interactRange)
        {
            int price = ChunkManager.Instance.GetNextPrice();
            int coins = ShopManager.Instance.GetCoins();

            if (pricePopup != null)
            {
                pricePopup.gameObject.SetActive(true);
                pricePopup.text = coins >= price
                    ? $"<color=green>{price} rs.</color>"
                    : $"<color=red>{price} rs.</color>";
            }

            Debug.Log($"[Chunk] Unlock cost: {price} | You have: {coins}");

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (ChunkManager.Instance.CanAfford())
                {
                    ChunkManager.Instance.UnlockChunk(borderObject);
                }
                else
                {
                    Debug.Log($"[Chunk] Not enough! Need {price}, have {coins}");
                }
            }
        }
        else
        {
            if (pricePopup != null)
                pricePopup.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}