using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject gameOverPanel;

    [Header("Gameplay UI")]
    public GameObject MiniMap;
    public GameObject PlayerHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // GAME OVER
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Hide gameplay UI
        if (MiniMap != null)
            MiniMap.SetActive(false);

        if (PlayerHealth != null)
            PlayerHealth.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // RESTART GAME
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // QUIT GAME
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}