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
    public Text questionText;
    public RawImage avatar;

    public Texture[] pics;
    public string[] infoText;

    public bool leftSideLayout;

    [Range(1, 10)]
    public int questAm;
    public int currentQuest;

	// Use this for initialization
	void Start ()
    {
        questAm = infoText.Length;
        questionText.text = infoText[currentQuest].ToString();
        leftSideLayout = true;
        Close();
	}

    /*void InsertValues(string[] inText, Texture[] inPics)
    {
        for (int i = 0; i < questAm; i++)
        {
            infoText[i] = inText[i].ToString();
            pics[i] = inPics[i];
        }
    }*/

    void FixedUpdate()
    {
        //Left side of the screen for the question box
        if (leftSideLayout == true)
        {
            avatar.transform.localPosition = new Vector3(-688.2f, -339.5f, 0);
        }

        //Right side of the screen for the question box
        if (leftSideLayout == false)
        {
            avatar.transform.localPosition = new Vector3(688.2f, -339.5f, 0);
        }

        //If we are at the first question, we cant go back anymore
        if(currentQuest == 0)
        {
            prevQuestionButton.SetActive(false);
        }

        //If we are at the last question, we cant go forward anymore
        if (currentQuest == questAm - 1)
        {
            nextQuestionButton.SetActive(false);
        }

        //If we are at question two or higher, we can go back
        if(currentQuest != 0)
        {
            prevQuestionButton.SetActive(true);
        }

        //If we are lower than the amount of question, we can go forth
        if (currentQuest != questAm - 1)
        {
            nextQuestionButton.SetActive(true);
        }
    }

    //Change avatar and text for the comming question
    public void NextQuestion()
    {
        currentQuest = currentQuest + 1;
        questionText.text = infoText[currentQuest].ToString();
        avatar.texture = pics[currentQuest];
    }

    //Change avatar and text for the previouse question
    public void PreviouseQuestion()
    {
        currentQuest = currentQuest - 1;
        questionText.text = infoText[currentQuest].ToString();
        avatar.texture = pics[currentQuest];
    }

    //When you wanna open the question box
    public void Open()
    {
        questionCanvas.SetActive(true);
        normalCanvas.SetActive(false);
    }

    //When you wanna close the question box
    public void Close()
    {
        questionCanvas.SetActive(false);
        normalCanvas.SetActive(true);
    }
}