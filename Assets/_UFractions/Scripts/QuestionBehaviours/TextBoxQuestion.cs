using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxQuestion : QuestionBehaviour
{
    [Serializable]
    public class TextBoxQuestionData : QuestionData
    {
        public List<TextBox> texts;

        public TextBoxQuestionData(List<TextBox> textBoxes)
        {
            this.texts = textBoxes;
        }
    }

    [Serializable]
    public class TextBox
    {
        public string avatar;
        public bool left;
        public string text;

        public TextBox(string avatar, bool leftPlacement, string text)
        {
            this.avatar = avatar;
            this.left = leftPlacement;
            this.text = text;
        }
    }

    public GameObject nextQuestionButton;
    public GameObject prevQuestionButton;
    public Text boxText;
    public RawImage avatarLeft;
    public RawImage avatarRight;

    public Texture[] pics;
    public string[] infoText;
    public bool[] leftOrRight;

    public bool leftSideLayout;

    public int questAm;
    public int currentQuest = 0;

    void avatarStateTrueFalse()
    {
        avatarRight.enabled = false;
        avatarLeft.enabled = true;
    }

    void avatarStateFalseTrue()
    {
        avatarLeft.enabled = false;
        avatarRight.enabled = true;
    }

    void avatarTextureUpdate()
    {
        avatarLeft.texture = pics[currentQuest];
        avatarRight.texture = pics[currentQuest];
    }

    //This is the function to input values for editor mode, you must have anything at all in it and you must have the same amount of avatars as questions / dialogs
    public void InsertValues(List<string> inText, List<Texture> inPics, List<bool> sideOfScreen)
    {
        if (inText.Count == 0 || inPics.Count == 0)
        {
            Debug.LogWarning("Oh hell no! Put some data in the text and avatars!");
        }

        if (inPics.Count != inText.Count)
        {
            Debug.LogWarning("You must have the same amount of avatars as questions / dialogs!");
        }

        infoText = new string[inText.Count];
        questAm = inText.Count;
        pics = new Texture[inPics.Count];
        leftOrRight = new bool[sideOfScreen.Count];

        for (int i = 0; i <= inText.Count - 1; i++)
        {
            infoText[i] = inText[i];
            pics[i] = inPics[i];
            leftOrRight[i] = sideOfScreen[i];
        }
    }

    private void Update()
    {
        //Left side of the screen for the question / dialog box
        if (leftSideLayout == true)
        {
            avatarStateTrueFalse();
        }

        //Right side of the screen for the question / dialog box
        if (leftSideLayout == false)
        {
            avatarStateFalseTrue();
        }
    }

    void FixedUpdate()
    {
        //If we are at the first question / dialog, we can't go back anymore
        if (currentQuest == 0)
        {
            prevQuestionButton.SetActive(false);
        }

        //If we are at the last question / dialog, we can't go forward anymore
        if (currentQuest == questAm - 1)
        {
            nextQuestionButton.SetActive(false);
        }

        //If we are at question / dialog two or higher, we can go back
        if (currentQuest != 0)
        {
            prevQuestionButton.SetActive(true);
        }

        //If we are lower than the total amount of questions / dialogs, we can go forth
        if (currentQuest != questAm - 1)
        {
            nextQuestionButton.SetActive(true);
        }
    }

    //Change avatar and text for the comming question / dialog
    public void NextQuestion()
    {
        currentQuest = currentQuest + 1;

        if (leftOrRight[currentQuest] == true)
        {
            leftSideLayout = true;
            avatarTextureUpdate();
        }

        else
        {
            leftSideLayout = false;
            avatarTextureUpdate();
        }
        boxText.text = infoText[currentQuest].ToString();
    }

    //Change avatar and text for the previouse question / dialog
    public void PreviouseQuestion()
    {
        currentQuest = currentQuest - 1;

        if (leftOrRight[currentQuest] == true)
        {
            leftSideLayout = true;
            avatarTextureUpdate();
        }

        else
        {
            leftSideLayout = false;
            avatarTextureUpdate();
        }

        boxText.text = infoText[currentQuest].ToString();
    }

    //When you wanna open the question / dialog box
    public override void Show() //SHOW 
    {
        //questionCanvas.SetActive(true);
        this.gameObject.SetActive(true); //This will be the canvas that is already in the AR scene
    }

    //When you wanna close the question / dialog box
    public override void Hide() //HIDE
    {
        //questionCanvas.SetActive(false);
        this.gameObject.SetActive(false); //This will be the canvas that is already in the AR scene
    }

    public override void SetupQuestion(QuestionData question)
    {
        List<Texture> textures = new List<Texture>();
        List<bool> leftOrientation = new List<bool>();
        List<string> texts = new List<string>();

        Texture texture = new Texture();

        bool fail = false;
        float time = 0;
        float timeout = 1;

        foreach (TextBox t in ((TextBoxQuestionData)question).texts)
        {
            WWW www = new WWW(StoryManager.Instance.GetImagePath(t.avatar));

            while (!www.isDone || !fail)
            {
                if (time >= timeout)
                    fail = true;
                time += Time.deltaTime;
            }

            texture = www.texture;

            textures.Add(texture);
            leftOrientation.Add(t.left);
            texts.Add(t.text);
        }

        InsertValues(texts, textures, leftOrientation);

        avatarTextureUpdate();
        questAm = infoText.Length;
        boxText.text = infoText[currentQuest].ToString();
        leftSideLayout = true;
        Hide();
    }

    private IEnumerator GetWebText(string url, Texture texture)
    {
        WWW www = new WWW(url);
        print(url);
        yield return new WaitForSeconds(1.0f);
        print("Texture Loaded");
        texture = www.texture;
    }
}
