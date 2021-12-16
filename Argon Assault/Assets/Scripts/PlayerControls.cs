using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;

    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down based upon player input")][SerializeField] float controlSpeed = 40f;
    [Tooltip("How fast player moves horizontally")][SerializeField] float xRange = 20f;
    [Tooltip("How fast player moves vertically")][SerializeField] float yRange = 18f;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -1f;
    [SerializeField] float positionYawFactor = 1f;

    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -14f;

    [Header("Laser gun array")]
    [SerializeField] GameObject[] lasers;

    float xThrow, yThrow;

    // OnEnable and OnDisable for new input system
    void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }
    void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }
    
    void ProcessRotation()
    {
        // rotation animation when moving
        float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controlPitchFactor;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessTranslation()
    {
        // read the input
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        // move the ship
        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float Zpos = transform.localPosition.z;
        // update pos
        float rawXpos = transform.localPosition.x + xOffset;
        float rawYpos = transform.localPosition.y + yOffset;
        // set the cap
        float clampedXPos = Mathf.Clamp(rawXpos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(rawYpos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, Zpos);

    }

    void ProcessFiring()
    {
        // if the button is pressed
        if (fire.ReadValue<float>() > 0.5)
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
