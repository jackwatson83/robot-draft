using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    PlayerMovement playerMovement;

    /// <summary>
    /// This game allows for, and requires in local multiplayer mode, different control schemes.
    /// Control schemes are scriptable objects created with a custom inspector window,
    /// and can be assigned in the editor (with the scriptable object asset) for modularity.
    /// </summary>
    [SerializeField]
    ControlScheme controls;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Input.GetKey(controls.Forward()))
        {
            playerMovement.PlayerForward();
        }
        if (Input.GetKey(controls.Left()))
        {
            playerMovement.PlayerLeft();
        }
        if (Input.GetKey(controls.Backwards()))
        {
            playerMovement.PlayerBack();
        }
        if (Input.GetKey(controls.Right()))
        {
            playerMovement.PlayerRight();
        }

        if (Input.GetKeyDown(controls.InteractKey()))
        {
            playerMovement.PlayerInteract();
        }
        if (Input.GetKeyUp(controls.InteractKey()))
        {
            playerMovement.CancelInteract();
        }
    }
}
