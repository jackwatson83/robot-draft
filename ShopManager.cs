using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopState {ACTIVE, INACTIVE}

/// <summary>
/// This script controls the shop portion of the game
/// The shop round is a timed round in which players run around collecting parts to add to their robot
/// </summary>
public class ShopManager : MonoBehaviour
{
    #region variables
    [Header("Camera")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 shopCameraPosition;

    [Header("Shop Time")]
    [SerializeField]
    float shopRoundTime;
    [SerializeField]
    float refillTime;

    [Header("BattleManager")]
    [SerializeField] RobotBattleManager BM;

    int refillCount;
    ShopState shopState;

    [Header("Part Pickup Info")]
    [SerializeField, Tooltip("This array holds all of the pickup objects in the scene")]
    GameObject[] pickupLocations;

    PartPickup[] pickupObjects;

    [SerializeField, Tooltip("This array holds all of the available parts in the game, to populate the shop with.")]
    RobotPart[] partArray;
    #endregion

    #region methods
    // Start is called before the first frame update
    void Start()
    {
        TogglePickupObjects();

        PartPickup[] temp = new PartPickup[pickupLocations.Length];
        //Get PartPickup Components
        for (int i = 0; i < pickupLocations.Length; i++)
        {            
            temp[i] = pickupLocations[i].GetComponent<PartPickup>();
        }

        pickupObjects = temp;

        StartCoroutine(ShopRound());
    }

    /// <summary>
    /// Sets the camera to look at the shop
    /// </summary>
    void SHOP_SetCameraPosition()
    {
        mainCamera.transform.position = shopCameraPosition;
    }

    /// <summary>
    /// This method changes what items are available in the shop.
    /// </summary>
    void RefillShop()
    {
        //Debug.Log("Refilling Shop");
        refillCount++;

        for (int i = 0; i < pickupObjects.Length; i++)
        {
            int r = Random.Range(0, partArray.Length);
            pickupObjects[i].SetPart(partArray[r]);
            //Debug.Log(partArray[r].componentName + " was selected [iteration " + refillCount + "]");
        }

        StartCoroutine(RefillTimer());
    }

    /// <summary>
    /// Coroutine to refill the shop after an amount of seconds (set in editor)
    /// </summary>
    /// <returns></returns>
    IEnumerator RefillTimer()
    {
        //Debug.Log("Waiting for Refill");

        yield return new WaitForSeconds(refillTime);

        //Debug.Log("timer up");
        if (shopState == ShopState.ACTIVE)
        {
            RefillShop();
        }        
    }

    /// <summary>
    /// Coroutine to set how long the game's shop round is active for (time set in editor)
    /// </summary>
    /// <returns></returns>
    IEnumerator ShopRound()
    {
        BM.battleState = BATTLESTATE.INACTIVE;

        Debug.Log("Shop Round Begin");
        shopState = ShopState.ACTIVE;

        SHOP_SetCameraPosition();

        RefillShop();

        yield return new WaitForSeconds(shopRoundTime);

        Debug.Log("Shop Round Over");
        TogglePickupObjects();
        shopState = ShopState.INACTIVE;

        BM.battleState = BATTLESTATE.ACTIVE;
        BM.StartBattleCountdown();
    }

    /// <summary>
    /// Activates the objects that are available in the shop
    /// </summary>
    void TogglePickupObjects()
    {
        //The objects are toggled on and off to help run the game smoothly
        // - to use less Instantiate or Destroy calls.
        // - pickup locations are the same objects each time; the item that is available for purchase
            // is tied to the mesh on display, as opposed to the entire gameobject
            // changing the item available in the shop is a mesh swap.
        for (int i = 0; i < pickupLocations.Length; i++)
        {
            pickupLocations[i].SetActive(!pickupLocations[i].activeInHierarchy);
        }
    }
    #endregion
}
