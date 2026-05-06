using UnityEngine;

public enum BoonEffectType { StatBoost, Heal, MaxHealthIncrease }

[CreateAssetMenu(fileName = "Boon", menuName = "Roguelite/Boon")]
public class BoonData : ScriptableObject
{
    public string boonName;
    [TextArea] public string description;
    public Sprite icon;

    public BoonEffectType effectType;

    [Header("Stat Boost")]
    public StatType statType;
    public int statValue;

    [Header("Heal / Max Health")]
    public int healAmount;
}
