using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI ScoreText;

    private void Update()
    {
        if (WaveManager.Instance != null)
        {
           // ScoreText.text = "nombre de bulle éclaté : " + GameManager.Instance.playerScore;
        }
    }
}
