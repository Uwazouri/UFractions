using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles user interaction and navigation in the main menu scene.
/// </summary>
public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject loadingPanel;
    public GameObject componentsPanel;

    public TMPro.TMP_InputField pathText;

    /// <summary>
    /// Set the local story folder path on start.
    /// </summary>
    public void Start()
    {
        this.pathText.text = StoryManager.Instance.GetLocalStoriesPath();
    }

    /// <summary>
    /// Go to the story selection scene.
    /// </summary>
    public void PlayGame()
    {
        this.componentsPanel.SetActive(false);
        this.loadingPanel.SetActive(true);
        SceneManager.LoadScene("StorySelectionScene");
    }

    /// <summary>
    /// Use to quit application.
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }

    /// <summary>
    /// Shows the main menu panel.
    /// </summary>
    public void ShowMainMenu()
    {
        this.optionsPanel.SetActive(false);
        this.mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Shows the options panel.
    /// </summary>
    public void ShowOptionsMenu()
    {
        this.mainMenuPanel.SetActive(false);
        this.optionsPanel.SetActive(true);

    }

    /// <summary>
    /// Clears custom stories from the local story folder.
    /// </summary>
    public void RemoveCustomStories()
    {
        StoryManager.Instance.ClearLocalStories();
    }
}
