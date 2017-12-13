using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PathSelection : MonoBehaviour
{
    public GameObject pathButtonPrefab;
    public GameObject pathSelectionCanvas;
    public GameObject pathChoiceContainer;
    public List<Story.Path> loadedPaths;
    public bool pathSelected;
    public string selectedPathName;
    public string selectedPathDescription;
    public Story.Path confirmPath;

    private GameObject pressedPath;

    //Run the path config
    void Start()
    {
        pathSelected = false;
        LoadPathConfig();
    }

    //If we have a selected path, then we will be able to interact with the confirm button to proceed
    private void Update()
    {
        if (pathSelected == false)
        {
            GameObject.Find("CONFIRM").GetComponent<Button>().interactable = false;
        }

        else
        {
            GameObject.Find("CONFIRM").GetComponent<Button>().interactable = true;
        }
    }

    /*
     * This is the function to update all the stories in the menu!
     * 
     * 1. We start by loading all the stories with assistance from StoryManager
     * 
     * 2. We go through all the loaded stories and create a button from a prefab. One button for each path
     * 
     * 3. We set the position, scale and parent for the button so it will align at the proper place
     * 
     * 4. We locate the name and the description text fields of the buttons, these will display name and description for their belonging path
     *  
     *      Example:
     *                  The adventure begins
     *                  As Chopper sets out to achive his life long dream, he takes a heartbreaking farewell to his hometown and walks out into the big world!
     *                  
     * 5. When all buttons have their rightful layout and texts, we position the container once more for a better fit
     */
    public void LoadPathConfig()
    {
        loadedPaths = StoryManager.Instance.GetAllPaths();

        for (int i = 0; i < loadedPaths.Count; i++)
        {
            GameObject pathButton = Instantiate(pathButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            pathButton.transform.parent = pathChoiceContainer.transform;
            pathButton.transform.localScale = new Vector3(1.0f, 1.1f, 1.0f);
            pathButton.transform.Find("pathNameText").GetComponent<Text>().text = loadedPaths[i].name;
            pathButton.transform.Find("pathDescriptionText").GetComponent<Text>().text = loadedPaths[i].pathEvent.ToString();
        }

        pathChoiceContainer.GetComponent<RectTransform>().localPosition = new Vector2(-150.0f, -1000.0f);
    }

    //Check witch button was pressed and retrive the belonging name and description
    //Then set the path linked to that button as the current one
    public void SelectedPath()
    {
        pressedPath = EventSystem.current.currentSelectedGameObject;

        selectedPathName = pressedPath.transform.Find("pathNameText").GetComponent<Text>().text;
        selectedPathDescription = pressedPath.transform.Find("pathDescriptionText").GetComponent<Text>().text;

        foreach (Story.Path s in this.loadedPaths)
        {
            if (s.name.Equals(selectedPathName))
            {
                confirmPath = s;
                pathSelected = true;
                break;
            }

            else
            {
                pathSelected = false;
            }
        }
    }

    //Lock the current path and proceed to next scene
    public void OpenPath()
    {
        if (pathSelected == true)
        {
            StoryManager.Instance.SetCurrentPath(confirmPath);
            SceneManager.LoadScene("PathProgressionScene");
        }
    }

    //Back to story selection
    public void GoBack()
    {
        SceneManager.LoadScene("StorySelectionScene");
    }
}