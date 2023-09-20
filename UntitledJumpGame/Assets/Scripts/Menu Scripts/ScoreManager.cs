using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //universal reference
    public static ScoreManager _instance;

    //MainMenu display variables
    [SerializeField] private TextMeshProUGUI highScore;
    private const string hiScore = "HighScore";

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
            highScore.SetText(PlayerPrefs.GetInt(hiScore) + "m");
        }
    }

    /// <summary>
    /// checks the new score, and updates the highscore if it has been beaten.
    /// </summary>
    /// <param name="score">New Score</param>
    public void NewScore(int score)
    {
        if (score > PlayerPrefs.GetInt(hiScore))
        {
            PlayerPrefs.SetInt(hiScore, score);
        }
    }
}
