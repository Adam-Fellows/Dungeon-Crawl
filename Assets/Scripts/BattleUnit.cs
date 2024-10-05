using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit
{
    public string unitName;

    public bool isAlive => currentHP > 0;

    public int initiative;
    public int maxHP;
    public int currentHP;
    public int defense;
    public int attackPower;

    public virtual void CalculateInitiative()
    {
    }

    public virtual void TakeDamage(int value)
    {
        //TODO: edge case for damage and defense = 0
        float damage = value * value / (value + defense);
        currentHP -= Mathf.RoundToInt(damage);
    }

    public virtual void DealDamage(BattleUnit targetUnit, int value)
    {
        targetUnit.TakeDamage(value);
    }
}
