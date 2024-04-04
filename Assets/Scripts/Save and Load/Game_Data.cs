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

    public SerialaizableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;


    public SerialaizableDictionary<string, float> volumeSettings;
    public Game_Data()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;

        this.currency = 0;
        skillTrea = new SerialaizableDictionary<string, bool>();
        inventory = new SerialaizableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerialaizableDictionary<string, bool>();


        volumeSettings = new SerialaizableDictionary<string, float>();
    }

}
