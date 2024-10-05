using System;
using System.Collections.Generic;
using UnityEngine;

//TODO: better implementation to get HP of player from here inot player battle unit in battle manager
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

    private const int maxLevel = 99;
    private const int maxVit = 40;
    private const int maxHP = 999;

    private const double a = 2.718; // Base of the logarithm (Euler's number for natural logarithm)
    private const double b = 4.0;  // Multiplier to control the growth rate

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) HP = CalculateHP(vitality, level);
    }

    public int CalculateHP(int vitality, int level)
    {
        // Ensure values are within the range
        vitality = Math.Clamp(vitality, 1, maxVit);  // Clamp vitality between 1 and MaxVit
        level = Math.Clamp(level, 1, maxLevel);      // Clamp level between 1 and MaxLevel

        // Calculate the HP using the logarithmic formula
        double vitalityFactor = (double)vitality / maxVit;
        double levelFactor = (double)level / maxLevel;

        // HP formula
        double hp = maxHP * Math.Log(b * (vitalityFactor * levelFactor) + 1, a);

        // Round and cast to integer
        return (int)Math.Round(hp);
    }
}
