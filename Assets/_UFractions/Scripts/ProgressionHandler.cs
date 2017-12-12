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
    
    // for debug
    //static bool start = false;

    // Use this for initialization
    /// <summary>
    ///  Checks if last problem is solved and then , depending on the number of events.
    ///  Takes you to the next one or if mutliple lets you choose wich path you want. 
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
             // display the slection of path , set scen to selected path 
            SpawnButton(); 
        }
	
	}


    /// <summary>
    /// Spawns the buttons when a the path braches to more then two. 
    /// loops thrugh the list of events , creat a button for each and set each button with a diffrent event.
    /// </summary>
    private void SpawnButton()
    {
        for (int i = 0; i < eventList.Count; i++)
        {
            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(buttonContainer,false);
            //go.GetComponent<Button>().onClick.AddListener(() => GoToScene(i));
            go.GetComponentInChildren<Text>().text = eventList[i].problemID.ToString();         
        }
    }

    /// <summary>
    /// Checks if the event button is pressed and set the confirmEvent to that buttons event 
    /// and set the eventSelected to true
    /// </summary>
    public void selectEvent()
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
    /// if the button is pressed play that sceen. 
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
