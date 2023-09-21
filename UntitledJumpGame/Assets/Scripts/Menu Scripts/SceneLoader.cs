using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene loader contains functions to load specific scenes via an integer or string, as well as an option to quit the game
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads the next scene using an int as a reference
    /// </summary>
    /// <param name="sceneNumber">int referencing the build number of the scene</param>
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    /// <summary>
    /// Loads the next scene using a String as a reference
    /// </summary>
    /// <param name="SceneName">string containing the scenes name</param>
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
