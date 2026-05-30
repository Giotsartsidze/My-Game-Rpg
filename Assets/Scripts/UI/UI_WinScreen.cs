using System.Collections;
using UnityEngine;
using TMPro;

public class UI_WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI metaEarnedText;

    [SerializeField] private float showDelay = 2f;

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowWithDelay());
    }

    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSecondsRealtime(showDelay);

        if (RunManager.instance == null) yield break;

        int baseEarned = Mathf.FloorToInt(RunManager.instance.runGold * 0.3f);
        int bonus = RunManager.instance.victoryBonusSouls;

        goldText.text      = $"Gold collected:  {RunManager.instance.runGold}";
        enemiesText.text   = $"Enemies slain:   {RunManager.instance.enemiesKilled}";
        timeText.text      = $"Time:            {RunManager.instance.GetFormattedTime()}";
        metaEarnedText.text = $"+{baseEarned + bonus} Souls  ({bonus} victory bonus!)";
    }

    // Wired to "Play Again" button in Inspector
    public void OnPlayAgain()
    {
        panel.SetActive(false);
        RunManager.instance.RestartRun();
    }
}
