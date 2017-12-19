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
        public List<ARObjectType> arObjects;

        public TextBoxQuestionData(List<TextBox> textBoxes, List<ARObjectType> arObjects)
        {
            this.texts = textBoxes;
            if (arObjects != null)
                this.arObjects = arObjects;
            else
                this.arObjects = new List<ARObjectType>();
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

    /*
     * NOTE:
     *  TrueFalse -> Left
     *  FalseTrue -> Right
     */

    //Set the avatar layout to left
    void AvatarStateTrueFalse()
    {
        avatarRight.enabled = false;
        avatarLeft.enabled = true;
    }

    //Set the avatar layout to right
    void AvatarStateFalseTrue()
    {
        avatarLeft.enabled = false;
        avatarRight.enabled = true;
    }

    //Update the textures, typically done when we change page 
    void AvatarTextureUpdate()
    {
        avatarLeft.texture = pics[currentQuest];
        avatarRight.texture = pics[currentQuest];
    }

    //This is the function to input values for editor mode, you must have anything at all in it and you must have the same amount of avatars as questions / dialogs
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

        //Takes the provided text and pictures, then assign them into seperate arrays
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

    //Check if the layout of the avatar is left or right, this is modifiable for each page
    private void Update()
    {
        //Left side of the screen for the text box
        if (leftSideLayout == true)
        {
            AvatarStateTrueFalse();
        }

        //Right side of the screen for the text box
        if (leftSideLayout == false)
        {
            AvatarStateFalseTrue();
        }
    }

    void FixedUpdate()
    {
        //If we are at the first page, we can't go back anymore
        if (currentQuest == 0)
        {
            prevQuestionButton.SetActive(false);
        }

        //If we are at the last page, we can't go forward anymore
        if (currentQuest == questAm - 1)
        {
            nextQuestionButton.SetActive(false);
        }

        //If we are at page two or higher, we can go back
        if (currentQuest != 0)
        {
            prevQuestionButton.SetActive(true);
        }

        //If we are lower than the total amount of pages, we can go forth
        if (currentQuest != questAm - 1)
        {
            nextQuestionButton.SetActive(true);
        }
    }

    //Change avatar and text for the comming page
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

    //Change avatar and text for the previouse page
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

    //When you wanna open the text box
    public override void Show()
    {
        this.gameObject.SetActive(true);
    }

    //When you wanna close the text box
    public override void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /*
     * This is the question setup
     * 
     * 1. We retreive data about witch question it currently is
     * 
     * 2. We search for the pictures linked to each page of text
     * 
     * 3. When the picture is retrived we will add it to that page and apply the text
     * 
     * 4. When all pics and texts are in place, we set the amount of questions so the orientation of witch question we are on works properly
     */
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

    //Retrieve textures from the internet
    private IEnumerator GetWebText(string url, Texture texture)
    {
        WWW www = new WWW(url);
        print(url);
        yield return new WaitForSeconds(1.0f);
        print("Texture Loaded");
        texture = www.texture;
    }
}