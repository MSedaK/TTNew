using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Slider _progressBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        // Skorlar� koruma: Skor sahne ge�i�lerinden �nce kaydedilir
        ScoreManager.Instance.SaveScores();

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _progressBar.value = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }

    public void ReloadScene()
    {
        // Skoru s�f�rla
        ScoreManager.Instance.ResetScore();

        // Ge�erli sahneyi yeniden y�kle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
