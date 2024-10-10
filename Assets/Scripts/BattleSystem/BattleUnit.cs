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
        Debug.Log(damage + " inflicted to " + unitName);
        currentHP -= Mathf.RoundToInt(damage);
        Debug.Log(unitName + "'s current health: " + currentHP);
    }

    public virtual void DealDamage(BattleUnit targetUnit, int value)
    {
        targetUnit.TakeDamage(value);
    }
}
