using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(001);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
