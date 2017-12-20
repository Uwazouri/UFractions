using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles path selection of the current story in the path selection scene.
/// Note: Needs current story to be set.
/// Note: Must set current path before it can continue to path progression scene.
/// </summary>
public class PathSelection : MonoBehaviour
{
    public GameObject pathButtonPrefab;
    public GameObject pathSelectionCanvas;
    public GameObject pathChoiceContainer;
    public GameObject loadingPanel;
    public GameObject buttonPanel;

    public List<Story.Path> loadedPaths;
    public bool pathSelected;
    public string selectedPathName;
    public string selectedPathDescription;
    public Story.Path confirmPath;

    private GameObject pressedPath;

    /// <summary>
    /// Set no path selected on start and load all paths from current story.
    /// </summary>
    void Start()
    {
        pathSelected = false;
        LoadPathConfig();
    }

    /// <summary>
    /// This is the function to update all the stories in the menu
    /// </summary>
    public void LoadPathConfig()
    {
        loadedPaths = StoryManager.Instance.GetAllPaths();

        for (int i = 0; i < loadedPaths.Count; i++)
        {
            GameObject pathButton = Instantiate(pathButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            pathButton.transform.SetParent(pathChoiceContainer.transform);
            pathButton.transform.localScale = new Vector3(1.0f, 1.1f, 1.0f);
            pathButton.transform.Find("pathNameText").GetComponent<Text>().text = loadedPaths[i].name;
            pathButton.transform.Find("pathDescriptionText").GetComponent<Text>().text = loadedPaths[i].description;
            Debug.Log(pathButton.transform.Find("pathNameText").GetComponent<Text>().text);
        }

        pathChoiceContainer.GetComponent<RectTransform>().localPosition = new Vector2(-150.0f, -1000.0f);
    }

    /// <summary>
    /// Check witch button was pressed and retrive the belonging name and description
    /// Set the path linked to the button as confirmed path
    /// </summary>
    public void selectedPath()
    {
        pressedPath = EventSystem.current.currentSelectedGameObject;

        selectedPathName = pressedPath.transform.Find("pathNameText").GetComponent<Text>().text;
        selectedPathDescription = pressedPath.transform.Find("pathDescriptionText").GetComponent<Text>().text;

        foreach (Story.Path s in this.loadedPaths)
        {
            print(s.name);
            if (s.name.Equals(selectedPathName))
            {
                confirmPath = s;
                pathSelected = true;
                GameObject.Find("CONFIRM").GetComponent<Button>().interactable = true;
                break;
            }

            else
            {
                pathSelected = false;
            }
        }
    }

    /// <summary>
    /// Sends the confirmed path to the PathSelection scene
    /// </summary>
    public void OpenPath()
    {
        if (pathSelected == true)
        {
            this.pathChoiceContainer.SetActive(false);
            this.buttonPanel.SetActive(false);
            this.loadingPanel.SetActive(true);
            StoryManager.Instance.SetCurrentPath(confirmPath);
            SceneManager.LoadScene("PathProgressionScene");
        }
    }

    /// <summary>
    /// When you wanna go back to the main menu
    /// </summary>
    public void GoBack()
    {
        SceneManager.LoadScene("StorySelectionScene");
    }
}