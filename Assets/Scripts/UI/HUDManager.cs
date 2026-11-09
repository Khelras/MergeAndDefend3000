using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject unsafeArea;
    [SerializeField] private GameObject pauseMenu;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;



    void Awake()
    {
        // Ensure references are set in the inspector
        if (unsafeArea == null) Debug.LogWarning("Unsafe Area GameObject is not assigned in the HUDManager.");
        if (pauseMenu == null) Debug.LogWarning("Pause Menu GameObject is not assigned in the HUDManager.");
        if (pauseMenuPanel == null) Debug.LogWarning("Pause Menu Panel GameObject is not assigned in the HUDManager.");
        if (settingsPanel == null) Debug.LogWarning("Settings Panel GameObject is not assigned in the HUDManager.");

        unsafeArea.SetActive(true);
        pauseMenu.SetActive(false);
        pauseMenuPanel.SetActive(false);
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
        // Pause Audio
        SFXManager.Instance.PauseAudio();

        // Pause Game
        pauseMenu.SetActive(true);
        ShowPauseMenu();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        // Resume Audio
        SFXManager.Instance.ResumeAudio();

        // Resume Game
        HideAllMenus();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        // Resume Game to resume any Paused State before going to Main Menu
        ResumeGame();

        // Go to the Main Menu Scene
        SceneManager.LoadScene("MainMenu");
    }
}