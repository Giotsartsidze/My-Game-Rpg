using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Game_Data 
{
    public int currency;

    public SerialaizableDictionary<string, bool> skillTrea;
    public SerialaizableDictionary<string, int> inventory;
    public List<string> equipmentId;
    public Game_Data()
    {
        this.currency = 0;
        skillTrea = new SerialaizableDictionary<string, bool>();
        inventory = new SerialaizableDictionary<string, int>();
        equipmentId = new List<string>();
    }
}
