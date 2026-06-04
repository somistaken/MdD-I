using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenUI : MonoBehaviour
{
    private static WinScreenUI instance;

    [Header("References")]
    [SerializeField] private GameObject winPanel;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public static WinScreenUI GetInstance()
    {
        return instance;
    }

    public void ShowWinScreen()
    {
        if (winPanel == null) return;

        winPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}