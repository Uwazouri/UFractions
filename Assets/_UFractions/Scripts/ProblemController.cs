using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
public class ProblemController : MonoBehaviour
{
    private QuestionBehaviour questionInterface;
    private AnswerBehaviour answerInterface;
    private bool setupDone = false;
    private bool answerOn = false;
    private bool questionOn = false;

	// Use this for initialization
	void Start ()
    {
        //StoryManager.Instance.DebugStory();
        StoryManager.Instance.SetCurrentStory("UFractions Demo Story");
        this.SetupProblem();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ToggleAnswerPart()
    {
        if (this.setupDone)
        {
            if (this.answerOn)
            {
                this.answerInterface.Hide();
                this.answerOn = false;
            }
            else
            {
                this.questionOn = false;
                this.questionInterface.Hide();
                this.answerInterface.Show();
                this.answerOn = true;
            }

        }
    }

    public void ToggleQuestionPart()
    {
        if (this.setupDone)
        {
            if (this.questionOn)
            {
                this.questionInterface.Hide();
                this.questionOn = false;
            }
            else
            {
                this.answerOn = false;
                this.answerInterface.Hide();
                this.questionInterface.Show();
                this.questionOn = true;
            }

        }
    }

    public void CommitAnswer()
    {
        if (this.setupDone)
        {
            if(this.answerInterface.GetResult())
            {
                // Handle Correct Answer
                print("Correct!");
            }
            else
            {
                // Handle wrong answer
                print("Wrong!");
            }
        }
    }

    private void SetupProblem()
    {
        this.answerInterface = StoryManager.Instance.GetCurrentAnswerBehaviour(this.transform);
        this.answerInterface.SetupAnswer(StoryManager.Instance.GetCurrentProblem().answer);
        this.questionInterface = StoryManager.Instance.GetCurrentQuestionBehaviour(this.transform);
        this.questionInterface.SetupQuestion(StoryManager.Instance.GetCurrentProblem().question);
        this.setupDone = true;
    }
}
