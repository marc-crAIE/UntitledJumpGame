using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    
    /// <summary>
    /// Enables this menu and sets all text to the scores
    /// </summary>
    public void InitialiseMenu()
    {
        scoreText.SetText(ScoreManager._instance.GetCurrentScore() + "m");
        highScoreText.SetText(ScoreManager._instance.GetHighScore() + "m");
        this.gameObject.SetActive(true);
    }
}
