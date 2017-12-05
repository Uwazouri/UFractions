﻿using System;
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
[RequireComponent(typeof(SlideUIElement))]
public class ChoiceAnswer : AnswerBehaviour
{
    [Serializable]
    public class ChoiceAnswerData : AnswerData
    {
        public List<string> textChoices;
        public List<string> imageChoices;
        public int answer;
        public int mode;

        public ChoiceAnswerData(List<string> strings, int answer, int mode)
        {
            this.mode = mode;
            if (mode == 0)
                this.imageChoices = strings;
            else if (mode == 1)
                this.textChoices = strings;
            this.answer = answer;
        }
    }

    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private Sprite debugSprite;

    private List<GameObject> choices;
    private GameObject answer;
    private GameObject lastChoice;

    /// <summary>
    /// Set the location of the panel and create a list for the choices.
    /// </summary>
    void Start ()
    {
        if (this.choices == null)
            this.choices = new List<GameObject>();
    }

    /// <summary>
    /// The virtual function that the problem can use to show the answer part.
    /// </summary>
    public override void Show()
    {
        GetComponent<SlideUIElement>().Show();
    }

    /// <summary>
    /// The virtual function that the problem can use to hide the answer part.
    /// </summary>
    public override void Hide()
    {
        GetComponent<SlideUIElement>().Hide();
    }

    /// <summary>
    /// Sets the choices and correct answer from a given problems answer.
    /// </summary>
    public override void SetupAnswer(AnswerData answer)
    {
        ChoiceAnswerData choicesAnswer = (ChoiceAnswerData)answer;

        if (choicesAnswer.mode == 0)
        {
            bool fail = false;
            float time = 0;
            float timeout = 1;

            foreach (string s in choicesAnswer.imageChoices)
            {
                WWW www = new WWW(StoryManager.Instance.GetImagePath(s));

                while (!www.isDone || !fail)
                {
                    if (time >= timeout)
                        fail = true;
                    time += Time.deltaTime;
                }

                Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.1f, 0.1f));

                this.AddChoice(sprite);
            }
        }

        if (choicesAnswer.mode == 1)
        {
            foreach (string s in choicesAnswer.textChoices)
            {
                this.AddChoice(s);
            }
        }

        this.SetAnswer(this.choices[(int)choicesAnswer.answer]);
        this.SetupChoices();
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
        if (this.choices == null)
            this.choices = new List<GameObject>();
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
        label.GetComponent<Image>().sprite = image;
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
        if (this.choices == null)
            this.choices = new List<GameObject>();
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

    /// <summary>
    /// Debug method for activating the result.
    /// </summary>
    public void DebugReslult()
    {
        if (this.GetResult())
        {
            print("CORRECT ANSWER!");
        }
        else
            print("FALSE ANSWER!");
    }
}
