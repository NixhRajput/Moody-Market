using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InventorySlot
{
    public string itemName = "";
    public SpriteRenderer iconRenderer;
    public SpriteRenderer slotRenderer;
}

public class InventorySystem : MonoBehaviour
{
    [Header("Slots (assign 10 in inspector)")]
    public InventorySlot[] slots = new InventorySlot[10];

    [Header("Sprites")]
    public Sprite normalSlotSprite;
    public Sprite activeSlotSprite;
    public Sprite carrotIcon;
    public Sprite carrotSeedIcon;
    public Sprite tomatoIcon;
    public Sprite tomatoSeedIcon;
    public Sprite wheatIcon;
    public Sprite wheatSeedIcon;

    private int activeSlot = 0;
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    void Start()
    {
        AddItem("Hoe");
        AddItem("CarrotSeed", 3);
        AddItem("TomatoSeed", 3);
        AddItem("WheatSeed", 3);
        SetActiveSlot(0);
    }

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SetActiveSlot(i);
        }
    }

    public void SetActiveSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        slots[activeSlot].slotRenderer.sprite = normalSlotSprite;
        activeSlot = index;
        slots[activeSlot].slotRenderer.sprite = activeSlotSprite;
    }

    public string GetActiveItemName() => slots[activeSlot].itemName;

    public void AddItem(string itemName, int count = 1)
    {
        if (itemCounts.ContainsKey(itemName)) itemCounts[itemName] += count;
        else itemCounts[itemName] = count;

        RefreshSlots();
        Debug.Log($"[Inventory] +{count} {itemName}");
    }

    public bool HasItem(string itemName)
    {
        return itemCounts.ContainsKey(itemName) && itemCounts[itemName] > 0;
    }

    public int GetItemCount(string itemName)
    {
        return itemCounts.ContainsKey(itemName) ? itemCounts[itemName] : 0;
    }

    public void RemoveItem(string itemName, int count = 1)
    {
        if (!HasItem(itemName)) return;
        itemCounts[itemName] -= count;
        if (itemCounts[itemName] <= 0) itemCounts.Remove(itemName);
        RefreshSlots();
        Debug.Log($"[Inventory] -{count} {itemName}");
    }

    void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].itemName = "";
            if (slots[i].iconRenderer != null)
            {
                slots[i].iconRenderer.sprite = null;
                slots[i].iconRenderer.gameObject.SetActive(false);
            }
        }

        int slotIndex = 0;
        foreach (var kv in itemCounts)
        {
            if (slotIndex >= slots.Length) break;
            if (kv.Value <= 0) continue;

            slots[slotIndex].itemName = kv.Key;
            Sprite icon = GetIconForItem(kv.Key);
            if (icon != null && slots[slotIndex].iconRenderer != null)
            {
                slots[slotIndex].iconRenderer.sprite = icon;
                slots[slotIndex].iconRenderer.gameObject.SetActive(true);
            }
            slotIndex++;
        }

        if (slots[activeSlot].slotRenderer != null)
            slots[activeSlot].slotRenderer.sprite = activeSlotSprite;
    }

    Sprite GetIconForItem(string itemName)
    {
        return itemName switch
        {
            "Carrot" => carrotIcon,
            "CarrotSeed" => carrotSeedIcon,
            "Tomato" => tomatoIcon,
            "TomatoSeed" => tomatoSeedIcon,
            "Wheat" => wheatIcon,
            "WheatSeed" => wheatSeedIcon,
            _ => null
        };
    }
}