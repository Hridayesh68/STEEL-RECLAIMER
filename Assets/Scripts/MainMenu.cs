using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    public void PlayGame()
    {
        Time.timeScale = 1f;
 Debug.Log("start game clicked");
        SceneManager.LoadScene(gameSceneName);

    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quit Game");
    }
}