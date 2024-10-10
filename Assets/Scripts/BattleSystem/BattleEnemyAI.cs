using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crawl.Control;

namespace Crawl.Battle
{
    public class BattleEnemyAI : MonoBehaviour
    {
        private bool isActive;

        private BattleUnit self;

        private List<BattleUnit> playerUnits = new List<BattleUnit>();

        private void Update()
        {
            if (isActive)
            {
                //TODO: coupling for enemy AI should be removed from update loop in future
                HandleEnemyAI();
            }
        }

        public void StartEnemyAI(BattleUnit self, List<BattleUnit> playerUnits)
        {
            isActive = true;
            this.self = self;
            this.playerUnits = playerUnits;
        }

        private void HandleEnemyAI()
        {
            //TODO: implement more complex behaviour (skills, parry, etc)
            Attack();
        }

        private void Attack()
        {
            int target = UnityEngine.Random.Range(0, playerUnits.Count - 1);
            self.DealDamage(playerUnits[target], self.attackPower);
            // End the enemy turn
            EndEnemyAI();
        }

        private void EndEnemyAI()
        {
            isActive = false;
            BattleManager.instance.EndEnemyTurn();
        }
    }
}
