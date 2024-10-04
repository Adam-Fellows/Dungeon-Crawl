using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit
{
    public string unitName;

    public float currentHP;

    public int vitality;        // Determines maximum health of the unit
    public int dexterity;       // Increases evasion and parry success chance, increases initiative
    public int strength;        // Increases attack power (pentrating, blunt, slash)
    public int endurance;       // Increases damage reduction
    public int intelligence;    // Increases attack power (skill, magic)
    public int luck;           // Increase evasion chance and initiative, increases skill success

    public int HP;
    public int attackPower;
    public int defense;
    public int initiative;

    public bool isAlive => currentHP > 0.0f;

    public BattleUnit(UnitStats stats)
    {
        this.unitName = stats.unitName;

        vitality = stats.vitality;
        dexterity = stats.dexterity;
        strength = stats.strength;
        strength = stats.strength;
        endurance = stats.endurance;
        intelligence = stats.intelligence;
        luck = stats.luck;
    }

    public BattleUnit(string name, int HP, int attackPower, int defense, int initiative)
    {
        this.unitName = name;

        this.HP = HP;
        this.attackPower = attackPower;
        this.defense = defense;
        this.initiative = initiative;

        this.currentHP = HP;
    }

    public void CalculateInitiative()
    {
        int dexterityModifier;
        if (dexterity < 8)
        {
            dexterityModifier = Mathf.RoundToInt(-0.1f * Mathf.Pow((dexterity - 6.0f), 3.0f) + 0.7f);
        }
        if (dexterity >= 14)
        {
            dexterityModifier = Mathf.RoundToInt(-0.015f * Mathf.Pow((dexterity - 12.1f), 0.3f) + (-1.0f));
        }
        else
        {
            dexterityModifier = 0;
        }

        // Get the initiative using the dexterity modifier
        initiative = Random.Range(0, 9) + dexterityModifier;
        // Ensure the initiative is never 0 and never exceeds 11
        initiative = Mathf.Clamp(initiative, 1, 11);

        Debug.Log(unitName + "'s initiative is " + initiative);
    }

    public void CalculateHP()
    {
        int vitalityModifier;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }

    public void Attack(BattleUnit target)
    {
        target.TakeDamage(attackPower);
    }
}
