using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit
{
    public string unitName;

    public int initiative;

    public virtual void CalculateInitiative()
    {
    }

    public virtual void TakeDamage(int value)
    {
    }

    public virtual void DealDamage(BattleUnit targetUnit, int value)
    {
        targetUnit.TakeDamage(value);
    }
}
