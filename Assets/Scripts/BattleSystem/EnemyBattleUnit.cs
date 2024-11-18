using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crawl.Data;

namespace Crawl.Battle
{
    public class EnemyBattleUnit : BattleUnit
    {
        public UnitStats enemyStats;

        public EnemyBattleUnit(UnitStats enemyStats)
        {
            this.enemyStats = enemyStats;

            this.unitName = enemyStats.unitName;
            this.maxHP = enemyStats.HP;
            this.currentHP = maxHP;
            this.attackPower = enemyStats.attackPower;
            this.defense = enemyStats.defense;
        }

        public override void CalculateInitiative()
        {
            base.CalculateInitiative();

            // Generate an enemy initiative range from 2 to 9
            initiative = Random.Range(0, 7) + 2;
            Debug.Log(unitName + "'s initiative is " + initiative);
        }
    }
}
