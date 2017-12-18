using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject aboutPanel;
    public Scene storySelection;
    public string testString;
    private AssetBundle myLoadedAssets;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayGame()
    {
        SceneManager.LoadScene("StorySelectionScene");
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
    public void ShowAbout()
    {
        this.HideMainMenu();
        this.aboutPanel.SetActive(true);
    }
    public void HideAbout()
    {
        this.aboutPanel.SetActive(false);
        this.ShowMainMenu();

    }
    public void ShowMainMenu()
    {
        this.mainMenuPanel.SetActive(true);
    }
    public void HideMainMenu()
    {
        this.mainMenuPanel.SetActive(false);
    }
    public void ShowHidePanel(string panelToShow)
    {

    }
}
