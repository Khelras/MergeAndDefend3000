using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject unsafeArea;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    void Awake()
    {
        // Ensure references are set in the inspector
        if (unsafeArea == null) Debug.LogWarning("Unsafe Area GameObject is not assigned in the MainMenuManager.");
        if (mainMenuPanel == null) Debug.LogWarning("Main Menu Panel GameObject is not assigned in the MainMenuManager.");
        if (settingsPanel == null) Debug.LogWarning("Settings Panel GameObject is not assigned in the MainMenuManager.");
        if (creditsPanel == null) Debug.LogWarning("Credits Panel GameObject is not assigned in the MainMenuManager.");

        unsafeArea.SetActive(true);
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
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
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAllMenus();
        mainMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAllMenus();
        settingsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        HideAllMenus();
        creditsPanel.SetActive(true);
    }

    public void PlayButtonExecute()
    {
        SceneManager.LoadScene("RobertsScene");
    }
}
