using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    [Header("Major stats")]
    public Stat strength; //incriese damage  by 1 point and crit power by 1%
    public Stat agility; // 1point increase evasion by 1% and crit by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point incriese health by 3 or 5 points

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;   // default value 150 %

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;


    public bool isIgnited; //does damage over time
    public bool isChilled; //reducee armor by 20%
    public bool isShocked; // reduce aaccuracy by 20%

    [SerializeField] private float ailmentsDuration = 4;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamagecooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;



    public int currentHealth;

    public System.Action onHealthChanged;

    protected virtual void  Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if(ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if(chilledTimer < 0)
        {
            isChilled = false;
        }

        if(shockedTimer < 0)
        {
            isShocked = false;
        }

        if(igniteDamageTimer < 0 && isIgnited)
        {

            DecressHealthBy(igniteDamage);

            if(currentHealth <= 0)
            {
                Die();
            }
            igniteDamageTimer = igniteDamagecooldown;
        }
    }

    

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }


        int totalDamage = damage.getValue() + strength.getValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetsArmor(_targetStats, totalDamage);
        //_targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.getValue();
        int _iceDamage = iceDamage.getValue();
        int _lightningDamage = lightningDamage.getValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.getValue();

        totalMagicalDamage = CheckTargetRessistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if(Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAlliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAlliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAlliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        _targetStats.ApplyAlliments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    private static int CheckTargetRessistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.getValue() + (_targetStats.intelligence.getValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAlliments(bool _ignite, bool _chill, bool _shock)
    {
        if(isIgnited || isChilled || isShocked)
        {
            return;
        }
        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);        }
        if (_chill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = _chill;

            float slowPercentage = .2f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }
        if (_shock)
        {
            shockedTimer = ailmentsDuration;
            isShocked = _shock;

            fx.ShockFxFor(ailmentsDuration);
        }
        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public virtual void TakeDamage(int _damage)
    {
        DecressHealthBy(_damage);
        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void DecressHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if(onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    protected virtual void Die()
    {
       // throw new System.NotImplementedException();
    }
    private bool  TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.getValue() + _targetStats.agility.getValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private int CheckTargetsArmor(CharacterStats _targetStats, int totalDamage)
    {

        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.getValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.getValue();
        }

        totalDamage -= _targetStats.armor.getValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.getValue() + agility.getValue();

        if(Random.Range(0, 100)<= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.getValue() + strength.getValue()) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.getValue() + vitality.getValue() * 5; ;
    }
}
