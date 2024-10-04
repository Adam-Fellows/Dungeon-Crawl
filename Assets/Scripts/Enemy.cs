using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<UnitStats> enemyUnits;

    private float distanceFromPlayer;

    private bool playerInRange;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceFromPlayer <= 2.15f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        //TODO: Pathfinding towards to player
        //TODO: Player turns to face enemy if enemy initiates attack from non-forward direction
        //Debug.Log("Player in range: " + playerInRange);
    }

    public void InitiateCombatPosition()
    {
        Vector3 targetPosition = player.position + (player.forward * 1.25f);
        StartCoroutine(MoveToPosition(targetPosition));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, 0.0f, 0.0f), 5.0f * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;  // Snap precisely to the target
    }
}
