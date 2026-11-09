using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    void Awake()
    {
        // Ensure references are set in the inspector
        if (pauseMenuPanel == null) Debug.LogWarning("Pause Menu Panel GameObject is not assigned in the PauseMenuManager.");
        if (settingsPanel == null) Debug.LogWarning("Settings Panel GameObject is not assigned in the PauseMenuManager.");

        pauseMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HideAllMenus()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        HideAllMenus();
        pauseMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAllMenus();
        settingsPanel.SetActive(true);
    }

    public void PauseGame()
    {
        ShowPauseMenu();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        HideAllMenus();
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}