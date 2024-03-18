using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;

    private Game_Data gameData;
    private List<ISaveManager> saveManagers;
    private FileDatahandler datahandler;


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        datahandler = new FileDatahandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }
    public void NewGame()
    {
        gameData = new Game_Data();
    }

    public void LoadGame()
    {
        gameData = datahandler.Load();

        if(this.gameData == null)
        {
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        datahandler.Save(gameData);
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}
