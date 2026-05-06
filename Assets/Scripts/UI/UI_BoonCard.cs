using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BoonCard : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;

    private BoonData currentBoon;

    public void Setup(BoonData boon)
    {
        currentBoon = boon;
        nameText.text = boon.boonName;
        descText.text = boon.description;

        if (boon.icon != null)
            icon.sprite = boon.icon;
    }

    // Wire this to the card's Button OnClick in the Inspector
    public void OnSelect()
    {
        BoonManager.instance.ApplyBoon(currentBoon);
        GetComponentInParent<UI_BoonSelection>().Hide();
    }
}
