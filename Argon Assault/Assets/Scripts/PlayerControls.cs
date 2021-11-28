using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] float controlSpeed = 40f;
    [SerializeField] float xRange = 20f;
    [SerializeField] float yRange = 18f;

    [SerializeField] float positionPitchFactor = -1f;
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float positionYawFactor = 1f;
    [SerializeField] float controlRollFactor = -14f;

    [SerializeField] InputAction fire;

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

        //Debug.Log("x: "+ xThrow);
        //Debug.Log("y: " + yThrow);
    }

    void ProcessFiring()
    {
        // if the button is pressed
        if (fire.ReadValue<float>() > 0.5)
        {
            ActivateLasers();
        }
        else
        {
            DeactivateLasers();
        }
    }

    void ActivateLasers()
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(true);
        }
    }

    void DeactivateLasers()
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
    }
}
