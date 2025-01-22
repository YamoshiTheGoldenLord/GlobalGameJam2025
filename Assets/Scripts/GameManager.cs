using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int playerStartLifeNumber = 3;
    private const string HighscoreKey = "Highscore";

    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI highScoreText;

    public enum GameState { MainMenu, StartGame, Playing, Paused, GameOver }
    public GameState currentState;

    public int playerScore = 0, playerHighScore, playerLife;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        LoadHighscore();
        playerLife = playerStartLifeNumber;
    }

    private void Update()
    {
        if (currentState == GameState.StartGame)
        {
            playerLife = playerStartLifeNumber;
            currentState = GameState.Playing;
        }

        if(highScoreText == null)
        {
            highScoreText = GameObject.Find("highScore").GetComponent<TextMeshProUGUI>();
        }

        if (playerScore > playerHighScore)
        {
            UpdateHighscore(playerScore);
        }
    }

    #region Functions

    #region scoring
    public void AddScore(int amount)
    {
        playerScore += amount;
        Debug.Log("Score: " + playerScore);
    }

    private void UpdateHighscore(int newHighscore)
    {
        if (newHighscore > playerHighScore)
        {
            playerHighScore = newHighscore;
            PlayerPrefs.SetInt(HighscoreKey, playerHighScore);
            PlayerPrefs.Save();
            UpdateHighscoreText(playerHighScore);
        }
    }

    private void LoadHighscore()
    {
        playerHighScore = PlayerPrefs.GetInt(HighscoreKey, 0);
        UpdateHighscoreText(playerHighScore);
    }

    private void UpdateHighscoreText(int highscore)
    {
        if (highScoreText != null)
        {
            highScoreText.text = "Highscore: " + highscore.ToString();
        }
    }
    #endregion

    #region Scene Management
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        playerScore = 0;
        playerLife = playerStartLifeNumber;
        currentState = GameState.StartGame;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    #endregion

    #endregion
}
