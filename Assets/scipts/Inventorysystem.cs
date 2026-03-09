using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class InventorySlot
{
    public string itemName = "";
    public SpriteRenderer iconRenderer;
    public SpriteRenderer slotRenderer;
    public TextMeshPro amountText;
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
    private Dictionary<string, int> itemSlotIndex = new Dictionary<string, int>();

    void Start()
    {
        AddItem("CarrotSeed", 3, 0);
        AddItem("TomatoSeed", 3, 1);
        AddItem("WheatSeed", 3, 2);
        SetActiveSlot(0);
    }

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SetActiveSlot(i);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) SetActiveSlot((activeSlot - 1 + slots.Length) % slots.Length);
        if (scroll < 0f) SetActiveSlot((activeSlot + 1) % slots.Length);
    }

    public void SetActiveSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        if (slots[activeSlot].slotRenderer != null)
            slots[activeSlot].slotRenderer.sprite = normalSlotSprite;
        activeSlot = index;
        if (slots[activeSlot].slotRenderer != null)
            slots[activeSlot].slotRenderer.sprite = activeSlotSprite;
    }

    public string GetActiveItemName() => slots[activeSlot].itemName;

    public void AddItem(string itemName, int count = 1, int fixedSlot = -1)
    {
        if (itemCounts.ContainsKey(itemName))
        {
            itemCounts[itemName] += count;
        }
        else
        {
            itemCounts[itemName] = count;

            if (fixedSlot >= 0)
                itemSlotIndex[itemName] = fixedSlot;
            else
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].itemName == "")
                    {
                        itemSlotIndex[itemName] = i;
                        break;
                    }
                }
            }
        }

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
        if (itemCounts[itemName] <= 0)
        {
            itemCounts.Remove(itemName);
            itemSlotIndex.Remove(itemName);
        }
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
            if (slots[i].amountText != null)
                slots[i].amountText.text = "";
        }

        foreach (var kv in itemCounts)
        {
            if (kv.Value <= 0) continue;
            if (!itemSlotIndex.ContainsKey(kv.Key)) continue;

            int idx = itemSlotIndex[kv.Key];
            if (idx >= slots.Length) continue;

            slots[idx].itemName = kv.Key;
            Sprite icon = GetIconForItem(kv.Key);

            if (icon != null && slots[idx].iconRenderer != null)
            {
                slots[idx].iconRenderer.sprite = icon;
                slots[idx].iconRenderer.gameObject.SetActive(true);
            }

            if (slots[idx].amountText != null)
                slots[idx].amountText.text = kv.Value > 1 ? kv.Value.ToString() : "";
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