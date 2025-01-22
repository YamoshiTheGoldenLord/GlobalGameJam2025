using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        if (WaveManager.Instance != null)
        {
           scoreText.text = "score : " + GameManager.Instance.playerScore;
            GameManager.Instance.highScoreText.text = "highScore : " + GameManager.Instance.playerHighScore;
        }
    }

    public void TogglePause()
    {
        if (GameManager.Instance.currentState == GameState.Playing)
        {
            GameManager.Instance.currentState = GameState.Paused;
            Time.timeScale = 0f;
        }
        else
        {
            GameManager.Instance.currentState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }
}
