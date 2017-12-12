using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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
            SceneManager.LoadScene("ProblemScene");
        }
        else if(eventList.Count == 0)
        {
            //Goes to PathSelection scene
            SceneManager.LoadScene("PathSelectionScene");
        }
        else if (eventList.Count == 1)
        {
            //if there is only one path, go to it 
            StoryManager.Instance.SetCurrentEvent(eventList[0]);
            SceneManager.LoadScene("ProblemScene");
        }
        else if (eventList.Count  >= 2)
        {
             // display the buttons for selection of events
            SpawnButton(); 
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
            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(buttonContainer,false);  
            go.GetComponentInChildren<Text>().text = eventList[i].problemID.ToString();     //Set the name of the button.    
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
        selectedEvent = buttonPressed.transform.Find("Text").GetComponent<Text>().text;

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
            StoryManager.Instance.SetCurrentEvent(confirmEvent); 
            SceneManager.LoadScene("ProblemScene");
        }
    }

}
