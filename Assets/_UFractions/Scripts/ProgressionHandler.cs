using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ProgressionHandler : MonoBehaviour
{
    float yPos = 0;
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    private List<Story.Event> eventList;
    private int count = 0 ;
    private GameObject buttonPressed;
    private string selectedEvent;
    private Story.Event confirmEvent;
    private  bool eventSelected = false;
    public GameObject loadingPanel;
    public Button continueButton;
    
    /// for debug
    //static bool start = false;

    /// <summary>
    ///  Checks if last problem is solved and then , depending on the number of events.
    ///  Takes you to the next one, if mutliple lets you choose wich path you want. 
    ///  Note: Seems to be realy slow when changing to scen with AR. 
    /// </summary>
    void Start ()
    {

        eventSelected = false;
        // Creat a list with all the next events. 
        eventList = StoryManager.Instance.GetCurrentEvent().nextEvents;

        //// just for debug 
        //if (!start)
        //{
        //    start = true;

        //    StoryManager.Instance.CreateDebugStory("dontcommit");
        //    StoryManager.Instance.SetCurrentPath(StoryManager.Instance.GetAllPaths()[3]);
        //}

        if (StoryManager.Instance.LastProblemSolved() == false)
        {
            // go back to current problem
            this.loadingPanel.SetActive(true);
            SceneManager.LoadScene("ProblemScene");
        }
        else if(eventList.Count == 0)
        {
            //Goes to PathSelection scene
            this.loadingPanel.SetActive(true);
            SceneManager.LoadScene("PathSelectionScene");
        }
        else if (eventList.Count == 1)
        {
            //if there is only one path, go to it
            this.loadingPanel.SetActive(true);
            StoryManager.Instance.SetCurrentEvent(eventList[0]);
            SceneManager.LoadScene("ProblemScene");
        }
        else if (eventList.Count  >= 2)
        {
             // display the buttons for selection of events
            SpawnButton();
            this.continueButton.gameObject.SetActive(true);
        }
	}


    /// <summary>
    /// Spawns the buttons when a the path branches. 
    /// Creat a button for each branch and set each button with that particular event. 
    /// Note: right now the text of the button is set to the ID of the path (problemID) later you might add a dscription and a name of the Event. 
    /// Improvements: some way to add a specific on klick function for each button.
    /// </summary>
    private void SpawnButton()
    {
        for (int i = 0; i < eventList.Count; i++)
        {
            GameObject go = Instantiate(buttonPrefab, buttonContainer);
            go.GetComponent<Button>().onClick.AddListener(() => { SelectEvent(); });
            go.GetComponentInChildren<TextMeshProUGUI>().text = eventList[i].problemID.ToString();     //Set the name of the button.    
        }
    }

    /// <summary>
    /// Checks wich of the buttons is pressed and set the event to that event. 
    /// 
    /// Improvements:  Not that modular with the use of Find() use some other way to instantiate the buttons with a specific funktion.  
    /// </summary>
    public void SelectEvent()
    {
        buttonPressed = EventSystem.current.currentSelectedGameObject;
        selectedEvent = buttonPressed.transform.Find("Text").GetComponent<TextMeshProUGUI>().text;

        foreach (Story.Event e in this.eventList)
        {

            if (e.problemID.ToString().Equals(selectedEvent))
            {
                // set the current event to the one in the button we klickt.               
                confirmEvent = e;
                eventSelected = true;
                break;
            }
            else
            {
                eventSelected = false;
            }

        }
    }

    /// <summary>
    /// If the button is pressed play that buttons event.  
    /// </summary>
    public void GoToScene()
    {
        if(eventSelected == true)
        {
            this.loadingPanel.SetActive(true);
            this.continueButton.gameObject.SetActive(false);
            this.buttonContainer.gameObject.SetActive(false);
            StoryManager.Instance.SetCurrentEvent(confirmEvent); 
            SceneManager.LoadScene("ProblemScene");
        }
    }

}
