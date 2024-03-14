using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Players Drop")]
    [SerializeField] private float chanceToLooseItem;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToUnequip = new List<InventoryItem>();

        foreach(InventoryItem item in inventory.GetEquipmentList())
        {
            if(Random.Range(0,100)<= chanceToLooseItem)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for(int i =0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }


        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materialsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.RemoveItem(materialsToUnequip[i].data );
        }
    }
}
