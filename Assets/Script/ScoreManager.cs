using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public const string CURRENT_SCORE = "CurrentScore";
    public const string HIGH_SCORE = "HighScore";

    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private AudioClip scoreSound;

    private int currentScore = 0;
    private int highScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE, 0);
        UpdateUI();

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            ResetScore();
        }
        else
        {
            currentScore = PlayerPrefs.GetInt(CURRENT_SCORE, 0);
            UpdateUI();
        }
    }

    public void AddScore()
    {
        currentScore++;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HIGH_SCORE, highScore);
        }
        PlayerPrefs.SetInt(CURRENT_SCORE, currentScore);
        UpdateUI();

        if (scoreSound != null)
            AudioSource.PlayClipAtPoint(scoreSound, Camera.main.transform.position);
    }

    public void ResetScore()
    {
        currentScore = 0;
        PlayerPrefs.SetInt(CURRENT_SCORE, currentScore);
        UpdateUI();
    }

    public void SaveScores()
    {
        PlayerPrefs.SetInt(HIGH_SCORE, highScore);
        PlayerPrefs.Save();
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
