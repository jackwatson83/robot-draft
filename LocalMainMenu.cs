using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script contains methods to be assigned to the buttons on the main menu.
/// </summary>
public class LocalMainMenu : MonoBehaviour
{
    /// <summary>
    /// starts the game
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
