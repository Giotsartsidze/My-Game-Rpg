using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strength;
    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private int currentHealth;
    // Start is called before the first frame update
    protected virtual void  Start()
    {
        currentHealth = maxHealth.getValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totalDamage = damage.getValue() + strength.getValue();
        _targetStats.TakeDamage(totalDamage);
    }
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
       // throw new System.NotImplementedException();
    }
}
