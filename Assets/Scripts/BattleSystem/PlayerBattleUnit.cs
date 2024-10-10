using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crawl.Battle
{
    public class PlayerBattleUnit : BattleUnit
    {
        public int dexterity;
        public int strength;
        public int endurance;
        public int intelligence;
        public int luck;

        public PlayerBattleUnit(string unitName, int HP, int dex, int str, int end, int intl, int lck)
        {
            this.unitName = unitName;

            this.maxHP = HP;
            this.currentHP = maxHP;
            this.dexterity = dex;
            this.strength = str;
            this.endurance = end;
            this.intelligence = intl;
            this.luck = lck;
        }

        public override void CalculateInitiative()
        {
            base.CalculateInitiative();

            //TODO: Calculate player initiative
        }
    }
}
