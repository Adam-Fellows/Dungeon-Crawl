using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Start()
        {
            SubscribeToInputEvents();
        }

        private void SubscribeToInputEvents()
        {
            // Movement input
            PlayerInput.instance.OnForwardPressed += Instance_OnForwardPressed;
            PlayerInput.instance.OnBackwardPressed += Instance_OnBackwardPressed;
            PlayerInput.instance.OnStrafeLeftPressed += Instance_OnStrafeLeftPressed;
            PlayerInput.instance.OnStrafeRightPressed += Instance_OnStrafeRightPressed;
            // Rotation input
            PlayerInput.instance.OnLookLeftPressed += Instance_OnLookLeftPressed;
            PlayerInput.instance.OnLookRightPressed += Instance_OnLookRightPressed;
        }

        private void Instance_OnLookRightPressed(object sender, EventArgs e)
        {
            if (GameManager.instance.GetGameState() != GameState.combat)
            {
                Rotate(90f);
            }
        }

        private void Instance_OnLookLeftPressed(object sender, EventArgs e)
        {
            if (GameManager.instance.GetGameState() != GameState.combat)
            {
                Rotate(-90f);
            }
        }

        private void Instance_OnStrafeRightPressed(object sender, EventArgs e)
        {
            if (CanMove(transform.right))
            {
                Move(Vector3.right);
            }
        }

        private void Instance_OnStrafeLeftPressed(object sender, EventArgs e)
        {
            if (CanMove(-transform.right))
            {
                Move(Vector3.left);
            }
        }

        private void Instance_OnBackwardPressed(object sender, EventArgs e)
        {
            if (CanMove(-transform.forward))
            {
                Move(Vector3.back);
            }
        }

        private void Instance_OnForwardPressed(object sender, EventArgs e)
        {
            if (CanMove(transform.forward))
            {
                Move(Vector3.forward);
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
            if (isRotating) { return; }

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
            if (isMoving)
            {
                return false;
            }

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
}
