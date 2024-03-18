using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDatahandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDatahandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    public void Save(Game_Data _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to file " + fullPath + "\n" + e);
        }
    }

    public Game_Data Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        Game_Data loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<Game_Data>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error on trying to load data from file : " + fullPath + "\n" + e);
            }
        }
        return loadData;
    }
}
