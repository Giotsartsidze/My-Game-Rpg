using System.Collections.Generic;
using UnityEngine;

public class UI_BoonSelection : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private UI_BoonCard[] cards;

    public void Show(List<BoonData> boons)
    {
        panel.SetActive(true);
        GameManager.instance.PauseGame(true);

        for (int i = 0; i < cards.Length; i++)
        {
            if (i < boons.Count)
            {
                cards[i].gameObject.SetActive(true);
                cards[i].Setup(boons[i]);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
        GameManager.instance.PauseGame(false);
    }
}
