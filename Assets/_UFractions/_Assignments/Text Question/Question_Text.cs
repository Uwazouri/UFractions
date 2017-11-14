using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question_Text : MonoBehaviour
{
    public GameObject normalCanvas;
    public GameObject questionCanvas;
    public GameObject closeButton;
    public GameObject openButton;
    public GameObject nextQuestionButton;
    public GameObject prevQuestionButton;
    public GameObject textBG;
    public Text boxText;
    public RawImage avatar;

    public Texture[] pics;
    public string[] infoText;

    public bool leftSideLayout;

    public int questAm;
    public int currentQuest;

	// Use this for initialization
	void Start ()
    {
        questAm = infoText.Length;
        boxText.text = infoText[currentQuest].ToString();
        leftSideLayout = true;
        Close();
	}

    //This is the function to input values for editor mode, you must have anything at all in it and you must have the same amount of avatars as questions / dialogs
    public void InsertValues(string[] inText, Texture[] inPics)
    {
        if (inText.Length == 0 || inPics.Length == 0)
        {
            Debug.LogWarning("Oh hell no! Put some data in the text and avatars!");
        }

        if (inPics.Length != inText.Length)
        {
            Debug.LogWarning("You must have the same amount of avatars as questions / dialogs!");
        }

        for (int i = 0; i < inText.Length; i++)
        {
            infoText[i] = inText[i].ToString();
            pics[i] = inPics[i];
        }
    }

    void FixedUpdate()
    {
        //Left side of the screen for the question / dialog box
        if (leftSideLayout == true)
        {
            avatar.transform.localPosition = new Vector3(-688.2f, -339.5f, 0);
        }

        //Right side of the screen for the question / dialog box
        if (leftSideLayout == false)
        {
            avatar.transform.localPosition = new Vector3(688.2f, -339.5f, 0);
        }

        //If we are at the first question / dialog, we can't go back anymore
        if(currentQuest == 0)
        {
            prevQuestionButton.SetActive(false);
        }

        //If we are at the last question / dialog, we can't go forward anymore
        if (currentQuest == questAm - 1)
        {
            nextQuestionButton.SetActive(false);
        }

        //If we are at question / dialog two or higher, we can go back
        if(currentQuest != 0)
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
        boxText.text = infoText[currentQuest].ToString();
        avatar.texture = pics[currentQuest];
    }

    //Change avatar and text for the previouse question / dialog
    public void PreviouseQuestion()
    {
        currentQuest = currentQuest - 1;
        boxText.text = infoText[currentQuest].ToString();
        avatar.texture = pics[currentQuest];
    }

    //When you wanna open the question / dialog box
    public void Open() //SHOW 
    {
        questionCanvas.SetActive(true);
        normalCanvas.SetActive(false); //This will be the canvas that is already in the AR scene
    }

    //When you wanna close the question / dialog box
    public void Close() //HIDE
    {
        questionCanvas.SetActive(false);
        normalCanvas.SetActive(true); //This will be the canvas that is already in the AR scene
    }
}