using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public List<BattleUnit> battleUnits = new List<BattleUnit>();

    public int turnIndex { get; private set; }

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
            //TODO: Rework enemy stats loading
            // Create a new battle unit with index stats
            EnemyBattleUnit unit = new EnemyBattleUnit(enemy[i]);
            // Calculates the units initiative
            unit.CalculateInitiative();
            // Add the unit to the list of battle units
            battleUnits.Add(unit);
        }

        // Add the player battle unit
        PlayerBattleUnit playerUnit = new PlayerBattleUnit("Player", 
            PlayerManager.instance.VIT(), PlayerManager.instance.DEX(), PlayerManager.instance.STR(),
            PlayerManager.instance.END(), PlayerManager.instance.INT(), PlayerManager.instance.LCK());
        battleUnits.Add(playerUnit);

        // Order the battle units list by initiative
        battleUnits = battleUnits.OrderBy(unit => unit.initiative).ToList();
        for (int i = 0; i < battleUnits.Count; i++)
        {
            Debug.Log(battleUnits[i].unitName + " is in battle queue at position " + i);
        }
    }

    private void InitiateTurn()
    {
        // Get the current battle unit
        BattleUnit currentUnit = battleUnits[turnIndex];

        if (currentUnit.isAlive)
        {
            if (currentUnit is PlayerBattleUnit)
            {
                // Inititate the plaeyr turn
                HandlePlayerTurn((PlayerBattleUnit)currentUnit);
            }
            else if (currentUnit is EnemyBattleUnit)
            {
                // Initiate the enemy turn
                HandleEnemyTurn((EnemyBattleUnit)currentUnit);
            }
        }
        //TODO: Update the turn index at some point
    }

    public void NextTurn()
    {
        // Get the next turn index
        turnIndex = (turnIndex + 1) % battleUnits.Count;
    }

    private void HandlePlayerTurn(PlayerBattleUnit player)
    {
        Debug.Log("Start " + player.unitName + "'s turn");
    }

    private void HandleEnemyTurn(EnemyBattleUnit enemy)
    {
        Debug.Log("Start " + enemy.unitName + "'s turn");
    }
    //((Accuracy - Dodge Chance)/Accuracy)*100 = hit chance calculation
}
