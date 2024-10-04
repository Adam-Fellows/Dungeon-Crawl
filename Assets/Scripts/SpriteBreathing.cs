using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBreathing : MonoBehaviour
{
    [SerializeField] private float scaleAmount = 0.1f;  // The amount the sprite will scale
    [SerializeField] private float speed = 2f;          // Speed of the breathing effect
    
    
    private Vector3 originalScale;

    private void Start()
    {
        // Store the original scale of the sprite
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Apply a smooth breathing effect using a sinusoidal function
        float scaleFactor = Mathf.Sin(Time.time * speed) * scaleAmount;

        // Set the sprite's scale while maintaining the original size and adding the breathing effect
        transform.localScale = originalScale + new Vector3(scaleFactor, scaleFactor, 0);
    }
}
