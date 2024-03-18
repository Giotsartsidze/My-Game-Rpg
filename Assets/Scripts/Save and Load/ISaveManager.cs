using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager 
{
    void LoadData(Game_Data _data);

    void SaveData(ref Game_Data _data);
}
