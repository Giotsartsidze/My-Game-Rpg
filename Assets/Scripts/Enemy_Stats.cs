using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats : CharacterStats
{
    private Enemy enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        enemy.DamageEffect();
    }
}
