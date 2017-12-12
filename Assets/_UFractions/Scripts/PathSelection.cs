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

    // Use this for initialization
    void Start()
    {
        pathSelected = false;
        LoadPathConfig();
    }

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

    //This is the function to update all the stories in the menu
    //Interface Factory will have to create a button with two texts, "pathNameText" & "pathDescriptionText"
    public void LoadPathConfig()
    {
        loadedPaths = StoryManager.Instance.GetAllPaths();

        for (int i = 0; i < loadedPaths.Count; i++)
        {
            GameObject pathButton = Instantiate(pathButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            pathButton.transform.parent = pathChoiceContainer.transform;
            pathButton.transform.localScale = new Vector3(1.0f, 1.1f, 1.0f);
            pathButton.transform.Find("pathNameText").GetComponent<Text>().text = loadedPaths[i].name;
            pathButton.transform.Find("pathDescriptionText").GetComponent<Text>().text = loadedPaths[i].description;
            Debug.Log(pathButton.transform.Find("pathNameText").GetComponent<Text>().text);
        }

        pathChoiceContainer.GetComponent<RectTransform>().localPosition = new Vector2(-150.0f, -1000.0f);
    }

    //Check witch button was pressed and retrive the belonging name and description
    //Set the path linked to the button as confirmed path
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
                break;
            }

            else
            {
                pathSelected = false;
            }
        }
    }

    //Sends the confirmed path to the PathSelection scene
    public void OpenPath()
    {
        if (pathSelected == true)
        {
            StoryManager.Instance.SetCurrentPath(confirmPath);
            SceneManager.LoadScene("PathProgressionScene");
        }
    }

    //When you wanna go back to the main menu
    public void GoBack()
    {
        SceneManager.LoadScene("StorySelectionScene");
    }
}