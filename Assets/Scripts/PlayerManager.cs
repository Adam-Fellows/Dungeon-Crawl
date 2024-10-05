using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public event EventHandler<OnEnemyEncountedEventArgs> OnEnemyEncounter;

    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private PlayerCameraEffects playerCameraEffects;

    public class OnEnemyEncountedEventArgs 
    {
        public List<UnitStats> enemyUnits;
    }

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

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraEffects = GetComponent<PlayerCameraEffects>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void EnemyEncounter(Enemy enemy, GameObject enemyObject)
    {
        enemyObject.GetComponent<Enemy>().InitiateCombatPosition();
        OnEnemyEncounter?.Invoke(this, new OnEnemyEncountedEventArgs { enemyUnits = enemy.enemyUnits });
    }

    public int VIT() => playerStats.vitality;

    public int DEX() => playerStats.dexterity;

    public int STR() => playerStats.strength;

    public int END() => playerStats.endurance;

    public int INT() => playerStats.intelligence;

    public int LCK() => playerStats.luck;

    public bool IsPlayerMoving()
    {
        return playerMovement.IsMoving();
    }
}
