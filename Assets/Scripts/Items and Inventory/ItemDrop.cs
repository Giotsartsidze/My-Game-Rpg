using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private itemData[] possibleDrop;
    private List<itemData> dropList =new List<itemData>();


    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for(int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0,100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for(int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count > 0)
            {
                itemData randomItem = dropList[Random.Range(0, dropList.Count)];
                dropList.Remove(randomItem);
                DropItem(randomItem);
            }
        }
    }
    protected void DropItem(itemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<itemObject>().SetupItem(_itemData, randomVelocity);
    }
}
