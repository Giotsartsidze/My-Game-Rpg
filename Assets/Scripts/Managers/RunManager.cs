using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour, ISaveManager
{
    public static RunManager instance;

    [Header("Run State")]
    public bool isRunActive;
    public int runGold;
    public int enemiesKilled;
    public float timeElapsed;
    public int roomsCleared;

    public int metaCurrency;

    [Header("Settings")]
    [SerializeField] private float goldToMetaRatio = 0.3f;
    public int victoryBonusSouls = 100;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isRunActive)
            timeElapsed += Time.deltaTime;
    }

    public void StartRun()
    {
        runGold = 0;
        enemiesKilled = 0;
        timeElapsed = 0f;
        roomsCleared = 0;
        isRunActive = true;
    }

    public void EndRun()
    {
        if (!isRunActive) return;

        isRunActive = false;

        int earned = Mathf.FloorToInt(runGold * goldToMetaRatio);
        metaCurrency += earned;
    }

    public void EndRunAsVictor()
    {
        EndRun();
        metaCurrency += victoryBonusSouls;
    }

    public void RestartRun()
    {
        SaveManager.instance?.SaveGame();
        StartRun();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void AddRunGold(int amount)
    {
        if (!isRunActive) return;
        runGold += amount;
    }

    public void RegisterEnemyKill()
    {
        if (!isRunActive) return;
        enemiesKilled++;
    }

    public string GetFormattedTime()
    {
        int m = Mathf.FloorToInt(timeElapsed / 60f);
        int s = Mathf.FloorToInt(timeElapsed % 60f);
        return $"{m:00}:{s:00}";
    }

    public void LoadData(GameData _data)
    {
        metaCurrency = _data.metaCurrency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.metaCurrency = metaCurrency;
    }
}
