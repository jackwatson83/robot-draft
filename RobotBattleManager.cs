using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public enum BATTLESTATE { ACTIVE, INACTIVE }

/// <summary>
/// This script manages the robot battle, 
/// handling starting the battle (configuring robot targets, camera setup etc)
/// Also handles ending the game once the battle is over.
/// </summary>
public class RobotBattleManager : MonoBehaviour
{
    #region variables
    [Header("Camera")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 arenaCameraPosition;

    [Header("UI")]
    [SerializeField] GameObject p1RobotView;
    [SerializeField] GameObject p2RobotView;
    [SerializeField] GameObject p1Panel;
    [SerializeField] GameObject p2Panel;
    [SerializeField] GameObject countdownPanel;
    [SerializeField] Text countdownText;
    [SerializeField] GameObject winnerCanvas;
    [SerializeField] Text winnerText;

    [Header("Robots")]
    [SerializeField] CustomisableRobot p1Robot;
    [SerializeField] CustomisableRobot p2Robot;

    [Space]
    [SerializeField] float countdownTime;
    public BATTLESTATE battleState;
    #endregion

    #region methods
    private void Awake()
    {
        //make sure the game starts outside of the battle state
        battleState = BATTLESTATE.INACTIVE;
    }

    /// <summary>
    /// Method that starts the robot battle.
    /// </summary>
    void StartBattle()
    {
        p1RobotView.SetActive(false);
        p2RobotView.SetActive(false);
        ARENA_SetCameraPosition();

        if (battleState == BATTLESTATE.ACTIVE)
        {
            Debug.Log("battle state active");

            p1Robot.SetTargetRobot(p2Robot);
            p2Robot.SetTargetRobot(p1Robot);
        }
    }

    /// <summary>
    /// This method begins the countdown before the battle
    /// </summary>
    public void StartBattleCountdown()
    {
        if (battleState == BATTLESTATE.ACTIVE)
        {
            StartCoroutine(BattleCountdown());
        }
    }

    /// <summary>
    /// Coroutine to handle starting the battle.
    /// </summary>
    /// <returns></returns>
    IEnumerator BattleCountdown()
    {
        p1Panel.SetActive(false);
        p2Panel.SetActive(false);
        countdownPanel.SetActive(true);
        countdownText.text = "ROBOTS, STANDBY";
        Debug.Log("Countdown begun");
        yield return new WaitForSeconds(countdownTime);

        //give players a chance to read
        countdownText.text = "ROBOTS, ACTIVATE";
        yield return new WaitForSeconds(1f);

        Debug.Log("robots, activate");
        battleState = BATTLESTATE.ACTIVE;
        StartBattle();
        countdownPanel.SetActive(false);
        p1Panel.SetActive(true);
        p2Panel.SetActive(true);
    }

    /// <summary>
    /// Sets the camera to look at the robot arena
    /// </summary>
    void ARENA_SetCameraPosition()
    {
        mainCamera.transform.position = arenaCameraPosition;
    }

    /// <summary>
    /// Method to be called after one robot loses
    /// </summary>
    public void EndBattle(GameObject winner)
    {
        battleState = BATTLESTATE.INACTIVE;
        p1Panel.SetActive(false);
        p2Panel.SetActive(false);

        winnerCanvas.SetActive(true);

        switch(winner.name)
        {
            case "Robot P1":
                winnerText.text = "Player 1 wins";
                break;
            case "Robot P2":
                winnerText.text = "Player 2 wins";
                break;
        }
    }

    /// <summary>
    /// Closes the game - paired with the Quit button on the Winner panel.
    /// </summary>
    public void CloseGame()
    {
        Application.Quit();
    }
    #endregion
}
