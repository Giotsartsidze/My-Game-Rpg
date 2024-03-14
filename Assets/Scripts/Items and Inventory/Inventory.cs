using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<itemData> startingEquipment;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<itemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<itemData, InventoryItem> stashDictionary;

    [Header("Inverntry UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSLotParent;

    private UI_itemSlot[] inventoryItemSlots;
    private UI_itemSlot[] stashItemSlot;
    private UI_Equipment_Slot[] equipmentSlot;
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

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_itemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_itemSlot>();
        equipmentSlot = equipmentSLotParent.GetComponentsInChildren<UI_Equipment_Slot>();
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            AddItem(startingEquipment[i]);
        }
    }

    public void EquipItem(itemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
            
        }
            if(oldEquipment != null)
            {
                UnequipItem(oldEquipment);
                AddItem(oldEquipment);
            }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifires();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifires();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }

        for(int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }


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

    public void RemoveItem(itemData _item, int quantity = 1)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= quantity)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.stackSize -= quantity;
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= quantity)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.stackSize -= quantity;
            }
        }

        UpdateSlotUI();
    }
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> craftingMaterials = new List<InventoryItem>();
        Dictionary<itemData, int> requiredMaterialCounts = new Dictionary<itemData, int>();

        // Calculate the required counts of each crafting material
        foreach (InventoryItem requiredMaterial in _requiredMaterials)
        {
            if (!requiredMaterialCounts.ContainsKey(requiredMaterial.data))
            {
                requiredMaterialCounts.Add(requiredMaterial.data, requiredMaterial.stackSize);
            }
            else
            {
                requiredMaterialCounts[requiredMaterial.data] += requiredMaterial.stackSize;
            }
        }

        // Check if there are enough of each required material in the stash
        foreach (var item in requiredMaterialCounts)
        {
            if (stashDictionary.TryGetValue(item.Key, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < item.Value)
                {
                    return false;
                }
                else
                {
                    craftingMaterials.Add(stashValue);
                }
            }
            else
            {
                return false;
            }
        }

        // Consume the required materials
        foreach (InventoryItem craftingMaterial in craftingMaterials)
        {
            RemoveItem(craftingMaterial.data, requiredMaterialCounts[craftingMaterial.data]);
        }

        // Add the crafted item
        AddItem(_itemToCraft);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;
    
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }
            return equipedItem;
    }
}
