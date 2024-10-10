using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraEffects : MonoBehaviour
{
    [SerializeField] private float bobFrequency = 6.0f;
    [SerializeField] private float bobHeight = 0.25f;
    [SerializeField] private float bobSwayAngle = 1f;

    private float timer = 0f;
    private float bobSideMovement = 0.0f;
    private float midpoint = 0f;
    private float lerpSpeed = 4.5f;

    private PlayerMovement playerMovement;

    private Transform cameraHolder;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        cameraHolder = transform.GetChild(0);

        initialLocalPosition = cameraHolder.localPosition;
        initialLocalRotation = cameraHolder.localRotation;
    }

    private void Update()
    {
        HandleCameraBob();
    }

    private void HandleCameraBob()
    {
        if (PlayerManager.instance.IsPlayerMoving())
        {
            timer += Time.deltaTime * bobFrequency;

            float bobX = Mathf.Sin(timer) * bobSideMovement;
            float bobY = Mathf.Sin(timer * 2) * bobHeight;

            Vector3 bobbingPosition = new Vector3(initialLocalPosition.x + bobX, initialLocalPosition.y + bobY, initialLocalPosition.z);
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, bobbingPosition, Time.deltaTime * lerpSpeed);

            Quaternion bobbingRotation = Quaternion.Euler(new Vector3(Mathf.Sin(timer * 2) * bobSwayAngle, initialLocalRotation.eulerAngles.y, initialLocalRotation.eulerAngles.z));
            cameraHolder.localRotation = Quaternion.Lerp(cameraHolder.localRotation, bobbingRotation, Time.deltaTime * lerpSpeed);
        }
        else
        {
            timer = 0f;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, initialLocalPosition, Time.deltaTime * lerpSpeed);
            cameraHolder.localRotation = Quaternion.Lerp(cameraHolder.localRotation, initialLocalRotation, Time.deltaTime * lerpSpeed);
        }
    }
}
