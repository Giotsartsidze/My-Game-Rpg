using System.Collections.Generic;
using UnityEngine;

public class BoonManager : MonoBehaviour
{
    public static BoonManager instance;

    [SerializeField] private List<BoonData> allBoons;
    [SerializeField] private UI_BoonSelection boonSelectionUI;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    public void OfferBoons()
    {
        if (allBoons == null || allBoons.Count == 0)
        {
            Debug.LogWarning("BoonManager: no boons assigned.");
            return;
        }

        List<BoonData> offered = PickRandom(3);
        boonSelectionUI.Show(offered);
    }

    public void ApplyBoon(BoonData boon)
    {
        CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        switch (boon.effectType)
        {
            case BoonEffectType.StatBoost:
                playerStats.GetStat(boon.statType).AddModifier(boon.statValue);
                break;

            case BoonEffectType.Heal:
                playerStats.IncreaseHealthBy(boon.healAmount);
                break;

            case BoonEffectType.MaxHealthIncrease:
                playerStats.maxHealth.AddModifier(boon.healAmount);
                playerStats.IncreaseHealthBy(boon.healAmount);
                break;
        }

        Debug.Log($"Boon applied: {boon.boonName}");
    }

    private List<BoonData> PickRandom(int count)
    {
        List<BoonData> pool = new List<BoonData>(allBoons);
        List<BoonData> result = new List<BoonData>();

        count = Mathf.Min(count, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        return result;
    }
}
