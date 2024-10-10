using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{ 
    normal, 
    combat 
}

namespace Crawl.Control
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameState gameState;

        [SerializeField] private CanvasGroup battleCanvasGroup;
        [SerializeField] private CanvasGroup generalCanvasGroup;

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
            SetGameState(GameState.normal);

            InactivateCanvasGroup(battleCanvasGroup);

            PlayerManager.instance.OnEnemyEncounter += Instance_OnEnemyEncounter;
        }

        private void Instance_OnEnemyEncounter(object sender, PlayerManager.OnEnemyEncountedEventArgs e)
        {
            InitiateCombat(e.enemyUnits);
        }

        public void InitiateCombat(List<UnitStats> enemy)
        {
            if (GetGameState() == GameState.combat)
            {
                Debug.Log("Combat cannot be initiated; already in combat.");
                return;
            }

            SetGameState(GameState.combat);
            // Send out events to initiate combat
            BattleManager.instance.InitiateCombat(enemy);

            // Update UI
            ActivateCanvasGroup(battleCanvasGroup);
            InactivateCanvasGroup(generalCanvasGroup);
        }

        private void ActivateCanvasGroup(CanvasGroup group)
        {
            group.interactable = true;
            group.alpha = 1;
        }

        private void InactivateCanvasGroup(CanvasGroup group)
        {
            group.interactable = false;
            group.alpha = 0;
        }

        private void SetGameState(GameState state)
        {
            gameState = state;
        }

        public GameState GetGameState()
        {
            return gameState;
        }
    }
}
