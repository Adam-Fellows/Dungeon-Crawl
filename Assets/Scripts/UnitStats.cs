using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Units/New Unit", fileName = "NewUnitStats")]
public class UnitStats : ScriptableObject
{
    public string unitName;

    // Stats
    public int vitality;        // Determines maximum health of the unit
    public int dexterity;       // Increases evasion and parry success chance, increases initiative
    public int strength;        // Increases attack power (pentrating, blunt, slash)
    public int endurance;       // Increases damage reduction
    public int intelligence;    // Increases attack power (skill, magic)
    public int luck;           // Increase evasion chance and initiative as well as item discovery

    public int HP;
    public int attackPower;
    public int defense;
    public int initiative;
}
