using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public List<BattleUnit> battleUnits = new List<BattleUnit>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    public void InitiateCombat(List<UnitStats> enemy)
    {
        battleUnits = new List<BattleUnit>();
        // Set up the list of battle units
        for (int i = 0; i < enemy.Count; i++)
        {
            // Create a new battle unit with index stats
            BattleUnit unit = new BattleUnit(enemy[i]);
            // Calculates the units initiative
            unit.CalculateInitiative();
            // Add the unit to the list of battle units
            battleUnits.Add(unit);
        }

        // Add the player battle unit
        BattleUnit playerUnit = new BattleUnit("Player", 80, 5, 5, 0); // PLACEHOLDER; replace with actual shit once player stats implemented
        battleUnits.Add(playerUnit);

        // Order the battle units list by initiative
        battleUnits = battleUnits.OrderBy(unit => unit.initiative).ToList();
    }
}
