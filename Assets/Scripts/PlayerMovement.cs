using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5.0f;         // Speed of forward/backward and left/right movement
    [SerializeField] private float rotateDuration = 0.2f;  // Duration for 90-degree rotation

    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask enemyMask;

    private float gridSize = 2.0f;          // The size of one grid tile
    private float collisionCheckDistance = 2.0f;

    private bool isMoving = false;
    private bool isRotating = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Update()
    {
        if (!isMoving && !isRotating)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CanMove(transform.forward))
            {
                Move(Vector3.forward);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CanMove(-transform.forward))
            {
                Move(Vector3.back);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GameManager.instance.GetGameState() != GameState.combat)
            {
                Rotate(-90f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (GameManager.instance.GetGameState() != GameState.combat)
            {
                Rotate(90f);
            }
        }
    }

    private void Move(Vector3 direction)
    {
        Vector3 moveDirection = transform.TransformDirection(direction);
        targetPosition = transform.position + moveDirection * gridSize;

        // Snap the target position to the grid
        targetPosition = SnapToGrid(targetPosition);

        // Start moving to the target position
        StartCoroutine(MoveToPosition());
    }

    private void Rotate(float angle)
    {
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angle, 0);

        // Start rotating
        StartCoroutine(RotateToAngle());
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;
        return position;
    }

    private IEnumerator MoveToPosition()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;  // Snap precisely to the target
        isMoving = false;
    }

    private IEnumerator RotateToAngle()
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotateDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }

    private bool CanMove(Vector3 direction)
    {
        if (GameManager.instance.GetGameState() == GameState.combat)
        {
            // Cannot move if combat is initiaited
            return false;
        }

        if (Physics.Raycast(transform.position, direction, out var hit, collisionCheckDistance, enemyMask))
        {
            Collider collider = hit.collider;
            Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log(enemy.name);
                // Initiate combat
                PlayerManager.instance.EnemyEncounter(enemy, enemy.gameObject);
                // Cannot move if initiating combat
                return false;
            }
        }

        if (Physics.Raycast(transform.position, direction, out hit, collisionCheckDistance, obstructionMask))
        {
            if (!hit.transform.gameObject.GetComponent<Collider>().isTrigger)
            {
                // Cannot move if obstructed by collider
                return false;
            }
        }
        // Can move if no obstruction is present
        return true;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward * collisionCheckDistance);
    }
}
