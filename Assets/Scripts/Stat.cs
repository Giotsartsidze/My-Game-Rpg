using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Stat 
{
    [SerializeField] private int baseValue;

    public List<int> modifires;

    public int getValue()
    {
        int finalValue = baseValue;

        foreach(int modifier in modifires)
        {
            finalValue += modifier;
        }


        return finalValue;
    }

    public void AddModifier(int _modifier)
    {
        modifires.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifires.RemoveAt(_modifier);
    }
}
