using System;
using System.Collections;
using UnityEngine;
using Crawl.Control;
using Crawl.General;

namespace Crawl.Player
{

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

        private void Instance_OnLookRightPressed(object sender, EventArgs e)
        {
            if (CanRotate())
            {
                Rotate(90f);
            }
        }

        private void Instance_OnLookLeftPressed(object sender, EventArgs e)
        {
            if (CanRotate())
            {
                Rotate(-90f);
            }
        }

        private void Instance_OnForwardPressed(object sender, EventArgs e)
        {
            BeginMove(Vector3.forward);
        }

        private void Instance_OnBackwardPressed(object sender, EventArgs e)
        {
            BeginMove(Vector3.back);
        }

        private void Instance_OnStrafeRightPressed(object sender, EventArgs e)
        {
            BeginMove(Vector3.right);
        }

        private void Instance_OnStrafeLeftPressed(object sender, EventArgs e)
        {
            BeginMove(Vector3.left);
        }

        private void BeginMove(Vector3 direction)
        {
            // Do checks to ensure the player can move in desired direction
            if (!MovingToObstruction(direction) && !MovingToEnemy(direction) && CanMove())
            {
                // Not moving onto enemy/obstruction so move
                Move(direction);
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

        private void EndMove()
        {
            // Send out events on player move end
        }

        private bool MovingToObstruction(Vector3 direction)
        {
            if (Physics.Raycast(transform.position, direction, out var hit, collisionCheckDistance, obstructionMask))
            {
                if (!hit.transform.gameObject.GetComponent<Collider>().isTrigger)
                {
                    // Moving onto an obstruction so return true
                    return true;
                }
            }
            // Not moving onto an obstruction so return false
            return false;
        }

        private bool MovingToEnemy(Vector3 direction)
        {
            if (Physics.Raycast(transform.position, direction, out var hit, collisionCheckDistance, enemyMask))
            {
                if (hit.transform.gameObject.GetComponent<Enemy>() != null)
                {
                    // Get the enemy from the hit game object
                    Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                    // Initiate combat if moving onto enemy
                    PlayerManager.instance.EnemyEncounter(enemy, enemy.gameObject);
                    // Moving onto the enemy
                    return true;
                }
            }
            // Not moving onto enemy so return false
            return false;
        }

        private bool CanMove()
        {
            if (GameManager.instance.GetGameState() != GameState.combat && !isMoving && !isRotating)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CanRotate()
        {
            if (GameManager.instance.GetGameState() != GameState.combat && !isMoving && !isRotating)
            {
                return false;
            }
            else
            {
                return true;
            }
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

        public bool IsMoving()
        {
            return isMoving;
        }

        private void OnEnable()
        {
            // Movement input events
            PlayerInput.instance.OnForwardPressed += Instance_OnForwardPressed;
            PlayerInput.instance.OnBackwardPressed += Instance_OnBackwardPressed;
            PlayerInput.instance.OnStrafeLeftPressed += Instance_OnStrafeLeftPressed;
            PlayerInput.instance.OnStrafeRightPressed += Instance_OnStrafeRightPressed;
            // Rotation input events
            PlayerInput.instance.OnLookLeftPressed += Instance_OnLookLeftPressed;
            PlayerInput.instance.OnLookRightPressed += Instance_OnLookRightPressed;
        }

        private void OnDisable()
        {
            // Movement input events
            PlayerInput.instance.OnForwardPressed -= Instance_OnForwardPressed;
            PlayerInput.instance.OnBackwardPressed -= Instance_OnBackwardPressed;
            PlayerInput.instance.OnStrafeLeftPressed -= Instance_OnStrafeLeftPressed;
            PlayerInput.instance.OnStrafeRightPressed -= Instance_OnStrafeRightPressed;
            // Rotation input events
            PlayerInput.instance.OnLookLeftPressed -= Instance_OnLookLeftPressed;
            PlayerInput.instance.OnLookRightPressed -= Instance_OnLookRightPressed;
        }
    }
}
