using System;

[Serializable]
public class InventoryItem
{
    public itemData data;
    public int stackSize;

    public InventoryItem(itemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}