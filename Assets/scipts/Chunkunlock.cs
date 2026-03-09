using UnityEngine;

public class ChunkUnlock : MonoBehaviour
{
    [Header("The border parent to destroy on unlock")]
    public GameObject borderObject;

    public float interactRange = 1f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (borderObject == null)
            borderObject = transform.parent.parent.gameObject;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= interactRange)
        {
            int price = ChunkManager.Instance.GetNextPrice();
            int coins = ShopManager.Instance.GetCoins();
            Debug.Log($"[Chunk] Unlock cost: {price} coins | You have: {coins} coins");

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (ChunkManager.Instance.CanAfford())
                {
                    ChunkManager.Instance.UnlockChunk(borderObject);
                }
                else
                {
                    Debug.Log($"[Chunk] Not enough coins! Need {price}, have {coins}");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}