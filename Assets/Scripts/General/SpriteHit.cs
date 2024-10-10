using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crawl.General.SpriteEffects
{
    public class SpriteHit : MonoBehaviour
    {
        [SerializeField] private float hitDuration = 0.2f;    // How long the hit effect lasts
        [SerializeField] private float shakeIntensity = 0.1f; // How much to shake the sprite
        [SerializeField] private float scaleMultiplier = 1.2f; // Scale multiplier when hit
        [SerializeField] private bool enableScale = false;    // Enable or disable scaling on hit

        [SerializeField] private Color hitColor = Color.red;  // The color to flash when hit

        private bool enableShake = true;     // Enable or disable shaking on hit

        private MeshRenderer meshRenderer;

        private Color originalColor;
        private Vector3 originalPosition;
        private Vector3 originalScale;

        private void Start()
        {
            // Cache the sprite renderer and original values
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            originalColor = meshRenderer.material.color;
            originalPosition = transform.localPosition;
            originalScale = transform.localScale;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TriggerHitEffect();
            }
        }

        public void TriggerHitEffect()
        {
            // Start the hit effect coroutine
            StartCoroutine(HitEffect());
        }

        private IEnumerator HitEffect()
        {
            // Change the sprite color to the hit color
            meshRenderer.material.color = hitColor;

            // Optionally shake the sprite
            if (enableShake)
            {
                StartCoroutine(Shake());
            }

            // Optionally scale the sprite
            if (enableScale)
            {
                transform.localScale = originalScale * scaleMultiplier;
            }

            // Wait for the hit duration
            yield return new WaitForSeconds(hitDuration);

            // Reset color, position, and scale to original values
            meshRenderer.material.color = originalColor;
            transform.localPosition = originalPosition;
            transform.localScale = originalScale;
        }

        private IEnumerator Shake()
        {
            float elapsedTime = 0f;

            while (elapsedTime < hitDuration)
            {
                // Apply a random small shake to the sprite's local position
                Vector3 randomPoint = originalPosition + (Vector3)(Random.insideUnitCircle * shakeIntensity);
                transform.localPosition = randomPoint;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Reset to the original position after shaking
            transform.localPosition = originalPosition;
        }
    }
}
