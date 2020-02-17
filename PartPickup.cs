using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updated version of PartSlot.
/// Incorporates runtime mesh/material changes for pickup objects in the shop area.
/// This script also handles pickup behaviour, detecting the player objects
///     and sends information about the current part in the slot to the player's UI
/// </summary>
public class PartPickup : MonoBehaviour
{
    public float gizmoRadius;

    public RobotPart part;

    public MeshFilter meshFilter;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }

    /// <summary>
    /// Sets the pickup's part component
    /// </summary>
    /// <param name="p">The part to store in this pickup object</param>
    public void SetPart(RobotPart p)
    {
        part = p;
        UpdatePickup();
    }

    void SetMesh(Mesh m)
    {
        meshFilter.mesh = m;
    }

    void SetMaterial(Material mat)
    {
        this.gameObject.GetComponent<Renderer>().material = mat;
    }

    private void Update()
    {
        //UpdatePickup();
    }

    /// <summary>
    /// Sets the pickup objects mesh and material to match the values stored in the RobotPart
    /// </summary>
    void UpdatePickup()
    {
        SetMesh(part.partMesh);
        SetMaterial(part.partMaterial);
    }

    /// <summary>
    /// When a player is near to the object (within pickup range), this method displays
    ///     information on the player's UI about this item
    ///     as well as checks if the player wishes to pick up the object
    ///         if the player does, update the players robot with the new item (this one).
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("NEAR PICKUP OBJECT");
            PlayerMovement p = other.GetComponentInParent<PlayerMovement>();
            PlayerManager PM = other.GetComponentInParent<PlayerManager>();

            PM.PickupTextUI("Add " + part.componentName + " to robot");

            if(p.IsInteracting())
            {
                //Debug.Log("pick up time");
                CustomisableRobot robot = PM.GetRobot();
                
                switch(part.type)
                {
                    case PartType.Base:
                        robot.SetBase(part);
                        break;
                    case PartType.Body:
                        robot.SetBody(part);
                        break;
                    case PartType.Head:
                        robot.SetHead(part);
                        break;
                    case PartType.Weapon:
                        robot.SetWeapon(part);
                        break;
                }
                robot.UpdateRobot();
            }
        }        
    }

    /// <summary>
    /// When the player leaves the collision area of this object, remove the information from the player's UI
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerManager PM = other.GetComponentInParent<PlayerManager>();
            PM.HidePickupText();
        }
    }
}
