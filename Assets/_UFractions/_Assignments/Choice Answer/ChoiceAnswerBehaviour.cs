using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// An AnswerBehaviour that will display multiple customizable choices that are selectable and one of them is the answer.
/// 
/// Attach the object to a panel in a canvas that is of suitable size.
/// Select a prefab of a choice that contains a Toggle component and is of suitable size according to the attached panel.
/// </summary>
public class ChoiceAnswerBehaviour : AnswerBehaviour
{
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private Sprite debugSprite;

    private List<GameObject> choices;
    private GameObject answer;
    private GameObject lastChoice;
    private bool show = false;
    private bool hide = false;

    /// <summary>
    /// Set the location of the panel and create a list for the choices.
    /// </summary>
    void Start ()
    {
        this.transform.localPosition = new Vector3(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMax, 0, 0);
        this.choices = new List<GameObject>();
        this.Setup();

        // Debug method for creating default choices since Setup currently does nothing.
        this.DebugSetup();
    }
	
    /// <summary>
    /// Update hides or shows the answer panel according to Hide and Show calls.
    /// </summary>
	void Update ()
    {
		if (this.show)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMax - GetComponent<RectTransform>().rect.width, 0, 0), 25);
            if (this.transform.localPosition.x <= GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMax - GetComponent<RectTransform>().rect.width)
                this.show = false;
        }
        else if (this.hide)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMax, 0, 0), 25);
            if (this.transform.localPosition.x >= GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMax)
                this.hide = false;
        }
	}

    /// <summary>
    /// The virtual function that the problem can use to show the answer part.
    /// </summary>
    public override void Show()
    {
        if (!this.hide)
            this.show = true;
    }

    /// <summary>
    /// The virtual function that the problem can use to hide the answer part.
    /// </summary>
    public override void Hide()
    {
        if (!this.show)
            this.hide = true;
    }

    /// <summary>
    /// The virtual function that the problem can use to get the result of the answer.
    /// </summary>
    /// <returns>Returns true if the question has been answered correctly given the context and setup of the answer.</returns>
    public override bool GetResult()
    {
        bool correct = false;

        foreach (GameObject g in this.choices)
        {
            if (g.GetComponent<Toggle>().isOn)
            {
                if (g == this.answer)
                    correct = true;
                else
                    return false;
            }
        }
        return correct;
    }

    /// <summary>
    /// Sets the choices and correct answer from the StoryManager.
    /// </summary>
    private void Setup()
    {
        // Needs implementation of StoryManager first.
    }

    /// <summary>
    /// Adds a choice to the answer with text.
    /// </summary>
    /// <param name="text">The text to display with this choice.</param>
    private void AddChoice(string text)
    {
        // Get object and set text
        GameObject newChoice = this.GetChoiceObject();
        newChoice.GetComponentInChildren<Text>().text = text;

        // Create the event trigger for the choice to only allow one choice.
        EventTrigger.Entry del = new EventTrigger.Entry();
        del.eventID = EventTriggerType.PointerDown;
        del.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(this.CheckChoices);
        del.callback.AddListener(call);

        // Add the event trigger
        EventTrigger eventTrigger = newChoice.GetComponent<EventTrigger>();
        if (eventTrigger == null)
            newChoice.AddComponent<EventTrigger>();
        newChoice.GetComponent<EventTrigger>().triggers.Add(del);

        // Add the choice
        this.choices.Add(newChoice);
    }

    /// <summary>
    /// Adds a choice to the answer with image.
    /// </summary>
    /// <param name="image">The image sprite to display with this choice.</param>
    private void AddChoice(Sprite image)
    {
        // Get object and set image
        GameObject newChoice = this.GetChoiceObject();
        GameObject label = newChoice.GetComponentInChildren<Text>().transform.gameObject;
        DestroyImmediate(label.GetComponent<Text>());
        label.AddComponent<Image>();
        label.GetComponent<Image>().sprite = this.debugSprite;
        label.GetComponent<Image>().preserveAspect = true;

        // Create the event trigger for the choice to only allow one choice.
        EventTrigger.Entry del = new EventTrigger.Entry();
        del.eventID = EventTriggerType.PointerDown;
        del.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(this.CheckChoices);
        del.callback.AddListener(call);

        // Add the event trigger
        EventTrigger eventTrigger = newChoice.GetComponent<EventTrigger>();
        if (eventTrigger == null)
            newChoice.AddComponent<EventTrigger>();
        newChoice.GetComponent<EventTrigger>().triggers.Add(del);

        // Add the choice
        this.choices.Add(newChoice);
    }

    /// <summary>
    /// Positions the choices correctly in the panel.
    /// </summary>
    private void SetupChoices()
    {
        int order = this.choices.Count;
        float halfH = GetComponent<RectTransform>().rect.height / 2;
        float halfW = GetComponent<RectTransform>().rect.width / 2;
        float space = halfH*2 / order;
        float over = space / 2;
        foreach (GameObject g in this.choices)
        {
            float yPosition = space * order;
            g.transform.localPosition = new Vector3(halfW, yPosition-halfH-over, 0);
            order--;
        }
    }

    /// <summary>
    /// Set which choice is the correct answer.
    /// </summary>
    /// <param name="answer">The choice object which is correct.</param>
    private void SetAnswer(GameObject answer)
    {
        this.answer = answer;
    }

    /// <summary>
    /// Creates and returns an instance of the choice prefab.
    /// </summary>
    /// <returns>The created choice object.</returns>
    private GameObject GetChoiceObject()
    {
        return Instantiate(this.choicePrefab, this.transform);
    }

    /// <summary>
    /// Used by choice event triggers to disable all choices before new choice is selected.
    /// </summary>
    /// <param name="baseEvent">Base event data from trigger.</param>
    private void CheckChoices(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        foreach (GameObject g in this.choices)
            g.GetComponent<Toggle>().isOn = false;
    }

    // DEBUG METHODS

    private void DebugSetup()
    {
        this.AddChoice("Choice A");
        this.AddChoice("Choice B");
        this.AddChoice("Choice C");
        this.SetAnswer(this.choices[this.choices.Count - 1]);
        this.AddChoice("Choice D");
        this.AddChoice(this.debugSprite);
        this.SetupChoices();
    }

    public void DebugReslult()
    {
        if (this.GetResult())
            print("CORRECT ANSWER!");
        else
            print("FALSE ANSWER!");
    }
}
