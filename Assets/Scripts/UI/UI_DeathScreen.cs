using System.Collections;
using UnityEngine;
using TMPro;

public class UI_DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI metaEarnedText;

    [SerializeField] private float showDelay = 1.5f;

    public void Show()
    {
        // Must activate before StartCoroutine — coroutines can't run on inactive GameObjects
        gameObject.SetActive(true);
        StartCoroutine(ShowWithDelay());
    }

    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSecondsRealtime(showDelay);

        if (RunManager.instance == null) yield break;

        int earned = Mathf.FloorToInt(RunManager.instance.runGold * 0.3f);

        goldText.text   = $"Gold collected:  {RunManager.instance.runGold}";
        enemiesText.text = $"Enemies slain:   {RunManager.instance.enemiesKilled}";
        timeText.text   = $"Time:            {RunManager.instance.GetFormattedTime()}";
        metaEarnedText.text = $"+{earned} Souls";
    }

    // Wired to "Try Again" button in Inspector
    public void OnTryAgain()
    {
        panel.SetActive(false);
        RunManager.instance.RestartRun();
    }
}
