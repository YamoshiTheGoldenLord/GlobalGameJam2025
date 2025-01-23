using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int playerStartLifeNumber = 3;
    private const string HighscoreKey = "Highscore";

    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI highScoreText;
    public Image lifeImage;
    public Sprite[] lifeSprites;
    private Canvas GameOverScreen;

    public enum GameState { MainMenu, StartGame, Playing, Paused, GameOver }
    public GameState currentState;

    [HideInInspector] public int playerScore = 0, playerHighScore, playerLife;

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
        UpdateLifeUI();
    }

    private void Update()
    {

        if (currentState == GameState.StartGame)
        {
            playerLife = playerStartLifeNumber;
            currentState = GameState.Playing;
            UpdateLifeUI();
        }
        else if (currentState == GameState.GameOver)
        {
            GameOverScreen = Object.FindFirstObjectByType<Canvas>();

            GameOverScreen.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (highScoreText == null)
            highScoreText = GameObject.Find("highScore").GetComponent<TextMeshProUGUI>();

        if (lifeImage == null)
            lifeImage = GameObject.Find("AcidState").GetComponent<Image>();

        if (playerScore > playerHighScore)
            UpdateHighscore(playerScore);

        if (playerLife <= 0)
            currentState = GameState.GameOver;
    }

    #region Functions

    #region lifeGestion
    public void LoseLife()
    {
        if (playerLife > 0)
        {
            playerLife--;
            UpdateLifeUI();
            Debug.Log("Player lost a life. Remaining lives: " + playerLife);

            if (playerLife <= 0)
            {
                currentState = GameState.GameOver;
            }
        }
    }

    public void UpdateLifeUI()
    {

        if (lifeImage != null && lifeSprites.Length > playerLife)
        {
            Debug.Log("UpdateLife");
            lifeImage.color = new Color(255, 255, 255, 255);
            lifeImage.sprite = lifeSprites[playerLife];
        }   
    }
    #endregion

    #region Scoring
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
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
    #endregion

    #endregion
}
