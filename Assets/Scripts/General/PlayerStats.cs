using System;
using System.Collections.Generic;
using UnityEngine;

//TODO: better implementation to get HP of player from here into player battle unit in battle manager
public class PlayerStats : MonoBehaviour
{
    public int vitality;
    public int dexterity;
    public int strength;
    public int endurance;
    public int intelligence;
    public int luck;

    [SerializeField] private int level;
    [SerializeField] private int HP;

    private void Start()
    {
        InitialisePlayerStats();
    }

    private void Update()
    {
    }
    
    private void InitialisePlayerStats()
    {
        // Get the players base HP
        HP = GetBaseHP();
    }

    private int GetBaseHP()
    {
        float baseHP;
        float baseMin = 2;
        float baseMax = 8;

        float a = UnityEngine.Random.Range(1, 4);
        // Modifier for the base HP formula
        float modifier = UnityEngine.Random.Range(baseMin, baseMax);
        // 50% chance for either base HP calculation
        baseHP = a > 2 ? (10 + modifier) : (9 * (10 + modifier) / 10);
        // Return the rounded baseHP
        return Mathf.RoundToInt(baseHP);
    }

    public int PlayerHP()
    {
        return HP;
    }
}
