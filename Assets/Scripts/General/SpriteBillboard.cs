using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crawl.General.SpriteEffects
{
    public class SpriteBillboard : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start()
        {
            // Get the main camera in the scene
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Make the sprite face the camera with Y-axis restriction
            FaceCameraWithYAxisRestriction();
        }

        private void FaceCameraWithYAxisRestriction()
        {
            if (mainCamera != null)
            {
                // Get the direction from the sprite to the camera
                Vector3 direction = mainCamera.transform.position - transform.position;

                // Zero out the Y component of the direction to restrict Y-axis rotation
                direction.y = 0;

                // Check if there's any direction left to prevent NaN rotations
                if (direction.sqrMagnitude > 0.001f)
                {
                    // Rotate the sprite to face the camera only along the Y-axis
                    transform.rotation = Quaternion.LookRotation(-direction);
                }
            }
        }
    }
}
