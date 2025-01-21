using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI ScoreText;

    private float countdown = 0f;

    private void Update()
    {
        if (WaveManager.Instance != null)
        {
            countdown -= Time.deltaTime;
            waveText.text = "Vague: " + (WaveManager.Instance.currentWaveIndex + 1);
            //ScoreText.text = "nombre de bulle �clat� : " + GameManager.Instance.playerScore; fait bug les 2 autres en fonction de son placement dans l'�x�cution
            timerText.text = "Prochaine vague dans: " + Mathf.Clamp(countdown, 0, Mathf.Infinity).ToString("F1") + "s";
        }
    }

    public void SetCountdown(float time)
    {
        countdown = time;
    }
}
