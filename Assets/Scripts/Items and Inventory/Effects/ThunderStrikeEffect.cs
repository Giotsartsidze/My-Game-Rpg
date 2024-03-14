using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/item effect/Thunder strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thinderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThinderStrike = Instantiate(thinderStrikePrefab, _enemyPosition.position, Quaternion.identity);
        Destroy(newThinderStrike, 1);

    }
}
