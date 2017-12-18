using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorController : MonoBehaviour
{
    public class EditorProblem
    {
        public Dropdown questionDropdown;

        public EditorProblem()
        {
        }
    }


    public Story story;
    public TextMeshProUGUI nameText;
    public TMP_InputField pathText;

	// Use this for initialization
	void Start ()
    {
        pathText.text = StoryManager.Instance.GetLocalStoriesPath();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdatePathText()
    {
        //pathText.SetText(StoryManager.Instance.GetLocalStoriesPath());
    }

    public void SaveStory()
    {
        StoryManager.Instance.CreateDebugStory(this.nameText.text);
    }

    public void SetStoryName(string name)
    {
        story.SetName(name);
    }

    public void SetStoryDescription(string description)
    {
        story.SetDescription(description);
    }
}
