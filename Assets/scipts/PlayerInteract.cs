using UnityEngine;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 1.5f;

    private InventorySystem inventory;
    private HarvestEffect harvestEffect;
    private List<CropTile> allTiles = new List<CropTile>();

    void Start()
    {
        inventory = GetComponent<InventorySystem>();
        harvestEffect = GetComponent<HarvestEffect>();
        RefreshTileList();
    }

    public void RefreshTileList()
    {
        allTiles.Clear();
        allTiles.AddRange(FindObjectsByType<CropTile>(FindObjectsSortMode.None));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryInteract();
    }

    void TryInteract()
    {
        CropTile closest = GetClosestTile();

        if (closest == null)
        {
            Debug.Log("No tile in range");
            return;
        }

        if (closest.state == TileState.Unplowed)
        {
            if (inventory.HasItem("Hoe"))
                closest.Plow();
            else
                Debug.Log("Need a Hoe!");
        }
        else if (closest.state == TileState.Plowed)
        {
            string activeItem = inventory.GetActiveItemName();

            CropType crop = activeItem switch
            {
                "CarrotSeed" => CropType.Carrot,
                "TomatoSeed" => CropType.Tomato,
                "WheatSeed" => CropType.Wheat,
                _ => CropType.None
            };

            if (crop != CropType.None)
            {
                inventory.RemoveItem(activeItem);
                closest.Plant(crop);
            }
            else
            {
                Debug.Log("Select a seed slot to plant!");
            }
        }
        else if (closest.state == TileState.Stage3)
        {
            CropType harvestedType = closest.cropType;
            Vector3 tilePos = closest.transform.position;

            string cropName = closest.Harvest();
            if (cropName != null)
            {
                inventory.AddItem(cropName, 2);
                inventory.AddItem(cropName + "Seed", 2);
                if (harvestEffect != null)
                    harvestEffect.PlayHarvest(harvestedType, tilePos);
            }
        }
        else
        {
            Debug.Log("Still growing... (" + closest.state + ")");
        }
    }

    CropTile GetClosestTile()
    {
        CropTile closest = null;
        float closestDist = Mathf.Infinity;

        foreach (CropTile tile in allTiles)
        {
            if (tile == null) continue;
            float dist = Vector2.Distance(transform.position, tile.transform.position);
            if (dist <= interactRange && dist < closestDist)
            {
                closestDist = dist;
                closest = tile;
            }
        }
        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}