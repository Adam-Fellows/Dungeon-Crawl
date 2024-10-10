using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crawl.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput instance = null;

        //public float mouseSensitivity = 2.0f;

        public event EventHandler<EventArgs> OnForwardPressed;
        public event EventHandler<EventArgs> OnBackwardPressed;
        public event EventHandler<EventArgs> OnStrafeLeftPressed;
        public event EventHandler<EventArgs> OnStrafeRightPressed;
        public event EventHandler<EventArgs> OnLookLeftPressed;
        public event EventHandler<EventArgs> OnLookRightPressed;

        private PlayerInputActions playerInputActions;

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

            playerInputActions = new PlayerInputActions();
        }

        private void Update()
        {
            MovementInput();
            RotationInput();
        }

        private void MovementInput()
        {
            // Forward input events
            playerInputActions.Player.MoveForward.performed += ctx => OnForwardPressed?.Invoke(this, EventArgs.Empty);
            playerInputActions.Player.MoveBackward.performed += ctx => OnBackwardPressed?.Invoke(this, EventArgs.Empty);
            // Strafe input events
            playerInputActions.Player.StrafeLeft.performed += ctx => OnStrafeLeftPressed?.Invoke(this, EventArgs.Empty);
            playerInputActions.Player.StrafeRight.performed += ctx => OnStrafeRightPressed?.Invoke(this, EventArgs.Empty);
        }

        private void RotationInput()
        {
            // Rotation input events
            playerInputActions.Player.RotateLeft.performed += ctx => OnLookLeftPressed?.Invoke(this, EventArgs.Empty);
            playerInputActions.Player.RotateRight.performed += ctx => OnLookRightPressed?.Invoke(this, EventArgs.Empty);
        }

        public void SwitchControlSchemeKeyboard()
        {
        }

        public void SwitchControlSchemeGamepad()
        {
        }

        private void OnEnable()
        {
            playerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            playerInputActions.Player.Disable();
        }
    }
}
