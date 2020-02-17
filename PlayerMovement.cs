using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to handle player movement. Contains methods for all possible movement actions
/// These methods will be called externally, from the InputManager script.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    bool interacting = false;

    /// <summary>
    /// Move the player forward
    /// </summary>
    public void PlayerForward()
    {
        player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Move the player backwards
    /// </summary>
    public void PlayerBack()
    {
        player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Move the player left
    /// </summary>
    public void PlayerLeft()
    {
        player.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Move the player right
    /// </summary>
    public void PlayerRight()
    {
        player.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Turns on interact mode
    /// </summary>
    public void PlayerInteract()
    {
        //Debug.Log("INTERACT");
        //check if part is in front
        //if it is, 
        //add the part to the robot
        //swap part's mesh to the default mesh
        interacting = true;
    }

    /// <summary>
    /// Turns off interact mode
    /// </summary>
    public void CancelInteract()
    {
        interacting = false;
    }

    public bool IsInteracting()
    {
        return interacting;
    }
}
