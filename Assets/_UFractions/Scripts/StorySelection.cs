using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StorySelection : MonoBehaviour
{
    public GameObject storyButtonPrefab;
    public GameObject storySelectionCanvas;
    public GameObject storyChoiceContainer;
    public List<Story> loadedStories;
    public bool storySelected;
    public string selectedStoryName;
    public string selectedStoryDescription;
    public Story confirmStory;

    private GameObject pressedStory;

    // Use this for initialization
    void Start()
    {
        storySelected = false;
        LoadStoryConfig();
    }

    private void Update()
    {
        if (storySelected == false)
        {
            GameObject.Find("CONFIRM").GetComponent<Button>().interactable = false;
        }

        else
        {
            GameObject.Find("CONFIRM").GetComponent<Button>().interactable = true;
        }
    }

    //This is the function to update all the stories in the menu
    //Interface Factory will have to create a button with two texts, "StoryNameText" & "StoryDescriptionText"
    public void LoadStoryConfig()
    {
        loadedStories = StoryManager.Instance.GetLocalStories();

        for (int i = 0; i < loadedStories.Count; i++)
        {
            GameObject storyButton = Instantiate(storyButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            storyButton.transform.parent = storyChoiceContainer.transform;
            storyButton.transform.localScale = new Vector3(1.0f, 1.1f, 1.0f);
            storyButton.transform.Find("StoryNameText").GetComponent<Text>().text = loadedStories[i].name;
            storyButton.transform.Find("StoryDescriptionText").GetComponent<Text>().text = loadedStories[i].description;
            Debug.Log(storyButton.transform.Find("StoryNameText").GetComponent<Text>().text);
        }

        storyChoiceContainer.GetComponent<RectTransform>().localPosition = new Vector2(-150.0f, -1000.0f);
    }

    //Check witch button was pressed and retrive the belonging name and description
    //Set the story linked to the button as confirmed story
    public void selectedStory()
    {
        pressedStory = EventSystem.current.currentSelectedGameObject;

        selectedStoryName = pressedStory.transform.Find("StoryNameText").GetComponent<Text>().text;
        selectedStoryDescription = pressedStory.transform.Find("StoryDescriptionText").GetComponent<Text>().text;

        foreach (Story s in this.loadedStories)
        {
            if (s.name.Equals(selectedStoryName))
            {
                confirmStory = s;
                storySelected = true;
                break;
            }

            else
            {
                storySelected = false;
            }
        }
    }

    //Sends the confirmed story to the PathSelection scene
    public void OpenStory()
    {
        if (storySelected == true)
        {
            StoryManager.Instance.SetCurrentStory(confirmStory);
            SceneManager.LoadScene("PathSelectionScene");
        }
    }

    //When you wanna go back to the main menu
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}