using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("General Settings")]
    [Tooltip("Speed the ship moves around the screen")][SerializeField] float controlSpeed = 25f;
    [Tooltip("Horizontal bounds for the ship")][SerializeField] float xRange = 10f;
    [Tooltip("Vertical bounds for the ship")][SerializeField] float yRange = 10f;

    [Header("Weapons")]
    [Tooltip("Player lasers go here")][SerializeField] GameObject[] lasers;

    [Header("Screen position based ship angle")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = -2f;

    [Header("Player movement based ship angle")]
    [SerializeField] float controlPitchFactor = -2f;
    [SerializeField] float controlRollFactor = 2f;

    Vector2 moveInput;
    bool playerFiring;
    //bool oldFireValue;
    //bool fireChanged;
    float horizontalMovement, verticalMovement;

    void Start()
    {
        //oldFireValue = playerFiring;
    }

    private void OnFire(InputValue inputValue)
    {
        playerFiring = inputValue.isPressed;
    }

    private void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        horizontalMovement = moveInput.x;
        verticalMovement = moveInput.y;
    }

    void Update()
    {
        //CheckFirePressed();
        ProcessTranslation();
        ProcessRotation();
        ProcessWeapons();
    }

    //private void CheckFirePressed()
    //{
    //    if (oldFireValue != playerFiring)
    //    {
    //        oldFireValue = !oldFireValue;
    //        fireChanged = true;
    //    }
    //    else
    //    {
    //        fireChanged = false;
    //    }
    //}

    private void ProcessWeapons()
    {
        if (playerFiring)
        {
            SetLasers(true);
        }
        else
        {
            SetLasers(false);
        }
    }

    private void SetLasers(bool isActive)
    {
        foreach (var laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;

            emissionModule.enabled = isActive;
        }
    }

    private void ProcessRotation()
    {
        float positionPitch = transform.localPosition.y * positionPitchFactor;
        float controlPitch = verticalMovement * controlPitchFactor;

        float pitch = positionPitch + controlPitch;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = horizontalMovement * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {


        float xOffset = horizontalMovement * Time.deltaTime * controlSpeed;
        float yOffset = verticalMovement * Time.deltaTime * controlSpeed;

        float xPos = transform.localPosition.x + xOffset;
        float yPos = transform.localPosition.y + yOffset;

        float clampedXPos = Mathf.Clamp(xPos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(yPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }
}
