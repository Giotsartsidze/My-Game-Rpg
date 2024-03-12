using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> inventory;
    public Dictionary<itemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<itemData, InventoryItem> stashDictionary;

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;

    private UI_itemSlot[] inventoryItemSlots;
    private UI_itemSlot[] stashItemSlot;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<itemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<itemData, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_itemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_itemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }
    public void AddItem(itemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateSlotUI();
    }

    private void AddToStash(itemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(itemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(itemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }

            if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
            {
                if (stashValue.stackSize <= 1)
                {
                    stash.Remove(stashValue);
                    stashDictionary.Remove(_item);
                }
                else
                {
                    stashValue.RemoveStack();
                }
            }
            UpdateSlotUI();
        }
    }

}
