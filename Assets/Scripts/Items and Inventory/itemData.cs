using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class itemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
}
