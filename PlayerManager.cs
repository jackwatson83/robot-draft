using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to connect player components in the editor
/// </summary>
public class PlayerManager : MonoBehaviour
{
    #region variables
    [SerializeField]
    private CustomisableRobot playerRobot;

    public Text pickupText;
    #endregion

    #region methods
    public void Start()
    {
        HidePickupText();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The player's robot</returns>
    public CustomisableRobot GetRobot()
    {
        return playerRobot;
    }

    public void PickupTextUI(string PT)
    {
        pickupText.text = PT;
        pickupText.gameObject.SetActive(true);
    }

    public void HidePickupText()
    {
        pickupText.gameObject.SetActive(false);
    }
    #endregion
}
