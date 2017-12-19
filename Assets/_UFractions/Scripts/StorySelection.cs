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
    public GameObject loadingPanel;
    public GameObject buttonPanel;

    public List<Story> loadedStories;

    public bool storySelected;
    public string selectedStoryName;
    public string selectedStoryDescription;

    public Story confirmStory;

    private GameObject pressedStory;

    /// <summary>
    /// Run the story config
    /// </summary>
    void Start()
    {
        storySelected = false;
        LoadStoryConfig();
    }

    //If we have a selected story, then we will be able to interact with the confirm button to proceed
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

    /*
     * This is the function to update all the stories in the menu!
     *
     * 1. We start by loading all the stories with assistance from StoryManager
     *
     * 2. We go through all the loaded stories and create a button from a prefab. One button for each story
     *
     * 3. We set the position, scale and parent for the button so it will align at the proper place
     *
     * 4. We locate the name and the description text fields of the buttons, these will display name and description for their belonging story
     *
     *      Example:
     *                  Adventures Of Chopper
     *                  This is a story about a little reindeer named Chopper, who's life long dream is to become the best doctor in the world!
     *  
     * 5. When all buttons have their rightful layout and texts, we position the container once more for a better fit
     */
    public void LoadStoryConfig()
    {
        loadedStories = StoryManager.Instance.GetLocalStories();

        for (int i = 0; i < loadedStories.Count; i++)
        {
            GameObject storyButton = Instantiate(storyButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            storyButton.transform.SetParent(storyChoiceContainer.transform);
            storyButton.transform.localScale = new Vector3(1.0f, 1.1f, 1.0f);
            storyButton.transform.Find("StoryNameText").GetComponent<Text>().text = loadedStories[i].name;
            storyButton.transform.Find("StoryDescriptionText").GetComponent<Text>().text = loadedStories[i].description;
        }

        storyChoiceContainer.GetComponent<RectTransform>().localPosition = new Vector2(-150.0f, -1000.0f);
    }

    //Check witch button was pressed and retrive the belonging name and description
    //Then set the story linked to that button as the current one
    public void SelectedStory()
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

    //Lock the current story and proceed to next scene
    public void OpenStory()
    {
        if (storySelected == true)
        {
            this.storyChoiceContainer.SetActive(false);
            this.buttonPanel.SetActive(false);
            this.loadingPanel.SetActive(true);
            StoryManager.Instance.SetCurrentStory(confirmStory);
            SceneManager.LoadScene("PathSelectionScene");
        }
    }

    //Back to main menu
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}