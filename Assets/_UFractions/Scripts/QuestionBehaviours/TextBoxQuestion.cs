using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Question Interface that uses textboxes to display a question.
/// </summary>
public class TextBoxQuestion : QuestionBehaviour
{
    /// <summary>
    /// The data interface for TextBoxQuestion.
    /// </summary>
    [Serializable] public class TextBoxQuestionData : QuestionData
    {
        public List<TextBox> texts;
        public List<ARObjectType> arObjects;

        /// <summary>
        /// Constructor that sets data.
        /// </summary>
        /// <param name="textBoxes">The textboxes that can be shown.</param>
        /// <param name="arObjects">List of ARObjects to spawn in AR scene.</param>
        public TextBoxQuestionData(List<TextBox> textBoxes, List<ARObjectType> arObjects)
        {
            this.texts = textBoxes;
            if (arObjects != null)
                this.arObjects = arObjects;
            else
                this.arObjects = new List<ARObjectType>();
        }
    }

    /// <summary>
    /// Contains data for how a TextBox behaves.
    /// </summary>
    [Serializable] public class TextBox
    {
        public string avatar;
        public bool left;
        public string text;

        /// <summary>
        /// Constructor that sets variables.
        /// </summary>
        /// <param name="avatar">The name of the image to display with this textbox. Null if no image.</param>
        /// <param name="leftPlacement">Is the image placed left.</param>
        /// <param name="text">The text of the textbox.</param>
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

    public Texture noImageTexture;

    public Texture[] pics;
    public string[] infoText;
    public bool[] leftOrRight;

    public bool leftSideLayout;

    public int questAm;
    public int currentQuest = 0;

    /*
     * NOTE:
     *  TrueFalse -> Left
     *  FalseTrue -> Right
     */

    /// <summary>
    /// Set the avatar layout to left.
    /// </summary>
    void AvatarStateTrueFalse()
    {
        avatarRight.enabled = false;
        avatarLeft.enabled = true;
    }

    /// <summary>
    /// Set the avatar layout to right.
    /// </summary>
    void AvatarStateFalseTrue()
    {
        avatarLeft.enabled = false;
        avatarRight.enabled = true;
    }

    /// <summary>
    /// Update the textures, typically done when we change page.
    /// </summary>
    void AvatarTextureUpdate()
    {
        avatarLeft.texture = pics[currentQuest];
        avatarRight.texture = pics[currentQuest];
    }

    /// <summary>
    /// This is the function to input values for editor mode, you must have anything at all in it and you must have the same amount of avatars as questions / dialogs.
    /// </summary>
    /// <param name="inText">Texts for the text boxes.</param>
    /// <param name="inPics">images for the text boxes.</param>
    /// <param name="sideOfScreen">placement of images in text boxes.</param>
    public void InsertValues(List<string> inText, List<Texture> inPics, List<bool> sideOfScreen)
    {
        if (inText.Count == 0 || inPics.Count == 0)
        {
            Debug.LogWarning("Put some data in the text and avatars!");
        }

        if (inPics.Count != inText.Count)
        {
            Debug.LogWarning("You must have the same amount of avatars as questions / dialogs!");
        }

        // Takes the provided text and pictures, then assign them into seperate arrays
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

    /// <summary>
    /// Check if the layout of the avatar is left or right, this is modifiable for each page.
    /// </summary>
    private void Update()
    {
        // Left side of the screen for the text box
        if (leftSideLayout == true)
        {
            AvatarStateTrueFalse();
        }

        // Right side of the screen for the text box
        if (leftSideLayout == false)
        {
            AvatarStateFalseTrue();
        }
    }

    /// <summary>
    /// Check movement permission in fixed update.
    /// </summary>
    void FixedUpdate()
    {
        // If we are at the first page, we can't go back anymore
        if (currentQuest == 0)
        {
            prevQuestionButton.SetActive(false);
        }

        // If we are at the last page, we can't go forward anymore
        if (currentQuest == questAm - 1)
        {
            nextQuestionButton.SetActive(false);
        }

        // If we are at page two or higher, we can go back
        if (currentQuest != 0)
        {
            prevQuestionButton.SetActive(true);
        }

        // If we are lower than the total amount of pages, we can go forth
        if (currentQuest != questAm - 1)
        {
            nextQuestionButton.SetActive(true);
        }
    }

    /// <summary>
    /// Change avatar and text for the comming page.
    /// </summary>
    public void NextQuestion()
    {
        currentQuest = currentQuest + 1;

        if (leftOrRight[currentQuest] == true)
        {
            leftSideLayout = true;
            AvatarTextureUpdate();
        }

        else
        {
            leftSideLayout = false;
            AvatarTextureUpdate();
        }

        boxText.text = infoText[currentQuest].ToString();
    }

    /// <summary>
    /// Change avatar and text for the previouse page.
    /// </summary>
    public void PreviouseQuestion()
    {
        currentQuest = currentQuest - 1;

        if (leftOrRight[currentQuest] == true)
        {
            leftSideLayout = true;
            AvatarTextureUpdate();
        }

        else
        {
            leftSideLayout = false;
            AvatarTextureUpdate();
        }

        boxText.text = infoText[currentQuest].ToString();
    }

    /// <summary>
    /// When you wanna open the text box.
    /// </summary>
    public override void Show()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// When you wanna close the text box.
    /// </summary>
    public override void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// This is the question setup
    /// 
    /// 1. We retreive data about witch question it currently is
    /// 
    /// 2. We search for the pictures linked to each page of text
    /// 
    /// 3. When the picture is retrived we will add it to that page and apply the text
    /// 
    /// 4. When all pics and texts are in place, we set the amount of questions so the orientation of witch question we are on works properly
    /// 
    /// </summary>
    /// <param name="question">The QuestionData of the problem.</param>
    public override void SetupQuestion(QuestionData question)
    {
        List<Texture> textures = new List<Texture>();
        List<bool> leftOrientation = new List<bool>();
        List<string> texts = new List<string>();

        foreach (TextBox t in ((TextBoxQuestionData)question).texts)
        {
            textures.Add(StoryManager.Instance.GetImageTexture(t.avatar));
            leftOrientation.Add(t.left);
            texts.Add(t.text);
        }

        InsertValues(texts, textures, leftOrientation);

        AvatarTextureUpdate();
        questAm = infoText.Length;
        boxText.text = infoText[currentQuest].ToString();
        leftSideLayout = this.leftOrRight[currentQuest];
        Hide();

        float y = 0;

        foreach (ARObjectType a in ((TextBoxQuestionData)question).arObjects)
        {
            InterfaceFactory.GetInstance().SpawnARObject(a, InterfaceFactory.GetInstance().transform, new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), y, UnityEngine.Random.Range(-5.0f, 5.0f)));
            y += 1.5f;
        }
    }
}