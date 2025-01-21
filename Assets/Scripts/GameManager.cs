using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int playerStartLifeNumber = 3;

    public static GameManager Instance { get; private set; }
    public enum GameState { MainMenu, StartGame, Playing, Paused, GameOver }
    public GameState currentState;

    public int playerScore = 0;
    public int playerLife;

    private void Awake()
    {
        #region(Singleton)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garde l'instance active entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Update()
    {
        if(currentState == GameState.StartGame)
        {
            playerLife = playerStartLifeNumber;
        }
    }
    #region(function)
    public void AddScore(int amount)
    {
        playerScore += amount;
        Debug.Log("Score: " + playerScore);
    }

    void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
        }
        else
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }

    #region(SceneGestion)
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #endregion
}
