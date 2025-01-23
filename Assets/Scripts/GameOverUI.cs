using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void RestartGameOver()
    {
        GameManager.Instance.RestartGame();
    }

    public void ReturnMenu()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }
}
