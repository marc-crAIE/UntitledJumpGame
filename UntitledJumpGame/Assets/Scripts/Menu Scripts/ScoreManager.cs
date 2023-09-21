using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    //universal reference
    public static ScoreManager _instance;

    //MainMenu display variables
    [SerializeField] private TextMeshProUGUI highScore;
    private const string HIS_CORE = "HighScore";


    //InGameScoring
    private float currentScore = 0;
    [SerializeField] private TextMeshProUGUI updatingScore;

    // Start is called before the first frame update
    void Start()
    {
        if (ScoreManager._instance && ScoreManager._instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;


        //update the main menu to display the highScore
        if (highScore)
        {
            highScore.SetText(PlayerPrefs.GetInt(HIS_CORE) + "m");
        }
    }

    /// <summary>
    /// checks the new score, and updates the highscore if it has been beaten.
    /// </summary>
    /// <param name="score">New Score</param>
    public void CheckScore()
    {
        if (currentScore > PlayerPrefs.GetInt(HIS_CORE))
        {
            PlayerPrefs.SetInt(HIS_CORE, (int)currentScore);
        }
    }

    /// <summary>
    /// Returns the current saved highScore
    /// </summary>
    /// <returns>int of the current high score</returns>
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIS_CORE);
    }

    /// <summary>
    /// Adds onto the current score
    /// </summary>
    /// <param name="scoreToAdd">amount to add to the score</param>
    public void AddScore(float scoreToAdd)
    {
        currentScore += scoreToAdd;
        updatingScore.SetText("" + (int)currentScore);
    }

    /// <summary>
    /// Returns the current score the player has
    /// </summary>
    /// <returns>integer of the players current score</returns>
    public int GetCurrentScore()
    {
        return (int)currentScore;
    }
}
