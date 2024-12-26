using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private GameObject scorePopupPrefab;

    private int currentScore = 0;
    private int highScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahneler arasý korunmasýný saðlar
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddScore()
    {
        currentScore++;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore); // En yüksek skoru kaydeder
        }
        UpdateUI();

        // Ses efekti çal
        if (scoreSound != null)
            AudioSource.PlayClipAtPoint(scoreSound, Camera.main.transform.position);

        // Puan popup göster
        if (scorePopupPrefab != null)
            Instantiate(scorePopupPrefab, scoreText.transform.position, Quaternion.identity);
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateUI();
    }

    public void SaveScores()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0); // En yüksek skoru yükler
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore.ToString();
        if (highScoreText != null)
            highScoreText.text = "Best: " + highScore.ToString();
    }

    public int GetCurrentScore() => currentScore;
    public int GetHighScore() => highScore;
}
