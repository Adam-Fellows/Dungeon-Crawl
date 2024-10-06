using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//TODO: visual feedback and pausing turns
//TODO: implement remaining player actions + battle end
//TODO: implement proper health formulas, damage formulas, hit chance/dodge chance
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public List<BattleUnit> battleUnits = new List<BattleUnit>();

    public int turnIndex { get; private set; }

    private int selectionIndex;

    private Transform battleUI;

    private BattleEnemyAI enemyAI;

    private BattleUnit selectedUnit;
    private BattleUnit currentUnit;

    private List<Button> playerActionButtons = new List<Button>();
    private List<BattleUnit> enemyUnits = new List<BattleUnit>();
    private List<BattleUnit> playerUnits = new List<BattleUnit>();

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
        enemyAI = GetComponent<BattleEnemyAI>();

        battleUI = GameObject.FindWithTag("BattleUI").transform;
        
        LoadPlayerButtonActions();
        DisablePlayerButtons();
    }

    private void Update()
    {
        HandleUnitSelection();
    }

    private void LoadPlayerButtonActions()
    {
        Transform playerButtonHolder = battleUI.GetChild(0);

        for (int i = 0; i < playerButtonHolder.childCount; i++)
        {
            playerActionButtons.Add(playerButtonHolder.GetChild(i).GetComponent<Button>());
        }
        // Update the functions of each button
        playerActionButtons[0].onClick.AddListener(PlayerButtonAttack);
    }

    public void InitiateCombat(List<UnitStats> enemy)
    {
        battleUnits = new List<BattleUnit>();
        // Set up the list of battle units
        for (int i = 0; i < enemy.Count; i++)
        {
            // Create a new battle unit with index stats
            EnemyBattleUnit unit = new EnemyBattleUnit(enemy[i]);
            // Calculates the units initiative
            unit.CalculateInitiative();
            // Add the unit to the list of battle units
            battleUnits.Add(unit);
            enemyUnits.Add(unit);
        }

        // Add the player battle unit
        PlayerBattleUnit playerUnit = new PlayerBattleUnit("Player", 
            PlayerManager.instance.HP(), PlayerManager.instance.DEX(), PlayerManager.instance.STR(),
            PlayerManager.instance.END(), PlayerManager.instance.INT(), PlayerManager.instance.LCK());
        battleUnits.Add(playerUnit);
        playerUnits.Add(playerUnit);

        // Order the battle units list by initiative
        battleUnits = battleUnits.OrderBy(unit => unit.initiative).ToList();
        for (int i = 0; i < battleUnits.Count; i++)
        {
            Debug.Log(battleUnits[i].unitName + " is in battle queue at position " + i);
        }
        // Inititate the combat by starting the first turn
        InitiateTurn();
    }

    private void InitiateTurn()
    {
        // Get the current battle unit
        currentUnit = battleUnits[turnIndex];

        if (currentUnit.isAlive)
        {
            if (currentUnit is PlayerBattleUnit)
            {
                // Inititate the player turn
                HandlePlayerTurn((PlayerBattleUnit)currentUnit);
                // Update the turn index for the next turn initiation
                NextTurn();
            }
            else if (currentUnit is EnemyBattleUnit)
            {
                // Initiate the enemy turn
                HandleEnemyTurn((EnemyBattleUnit)currentUnit);
                // Update the turn index for the next turn initiation
                NextTurn();
            }
        }
        else
        {
            if (currentUnit is EnemyBattleUnit)
            {
                enemyUnits.Remove(currentUnit);
            }
            // Update the turn index for the next turn initiation
            NextTurn();
            // Called when current unit is dead; move to next unit
            InitiateTurn();
            return;
        }
    }

    public void NextTurn()
    {
        // Get the next turn index
        turnIndex = (turnIndex + 1) % battleUnits.Count;
    }

    private void HandlePlayerTurn(PlayerBattleUnit player)
    {
        Debug.Log("Start " + player.unitName + "'s turn");
        // Activate the player action buttons
        EnablePlayerButtons();
    }

    private void HandleUnitSelection()
    {
        if (GameManager.instance.GetGameState() != GameState.combat) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // + enemyUnits.Count to prevent returing negative values
            selectionIndex = (selectionIndex - 1 + enemyUnits.Count) % enemyUnits.Count;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectionIndex = (selectionIndex + 1) % enemyUnits.Count;
        }
        // Get the unit that is the players target based on the selection index
        selectedUnit = enemyUnits[selectionIndex];
        Debug.Log("Player selected " +selectedUnit.unitName + " as target");
    }

    private void PlayerButtonAttack()
    {
        Debug.Log("Player attack " + selectedUnit.unitName);
        // raw damage = base + strength modifier
        int damage = 10;
        currentUnit.DealDamage(selectedUnit, damage);
        // End the players turn
        EndPlayerTurn();
    }

    private void EndPlayerTurn()
    {
        // Called after the player makes any terminating action
        InitiateTurn();
        // Disable the player action buttons
        DisablePlayerButtons();
    }

    private void EnablePlayerButtons()
    {
        for (int i = 0; i < playerActionButtons.Count; i++)
        {
            playerActionButtons[i].interactable = true;
        }
    }

    private void DisablePlayerButtons()
    {
        for (int i = 0; i < playerActionButtons.Count; i++)
        {
            playerActionButtons[i].interactable = false;
        }
    }

    private void HandleEnemyTurn(EnemyBattleUnit enemy)
    {
        Debug.Log("Start " + enemy.unitName + "'s turn");
        // Enable the enemy AI
        enemyAI.StartEnemyAI(enemy, playerUnits);
    }

    public void EndEnemyTurn()
    {
        // Optional behaviour upon ending enemy turn
        InitiateTurn();
    }

    //((Accuracy - Dodge Chance)/Accuracy)*100 = hit chance calculation
}
