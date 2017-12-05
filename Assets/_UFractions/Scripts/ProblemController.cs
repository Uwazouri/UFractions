using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
public class ProblemController : MonoBehaviour
{
    public Story mmm;
    private QuestionBehaviour questionInterface;
    private AnswerBehaviour answerInterface;
    private bool setupDone = false;
    private bool answerOn = false;
    private bool questionOn = false;

	// Use this for initialization
	void Start ()
    {
        StoryManager.Instance.CreateDebugStory("FakeStory");
        mmm = StoryManager.Instance.currentStory;
        //StoryManager.Instance.SetCurrentStory(StoryManager.Instance.GetLocalStories()[0]);
        StoryManager.Instance.SetCurrentPath(StoryManager.Instance.GetAllPaths()[2]);
        StoryManager.Instance.SetCurrentEvent(StoryManager.Instance.GetCurrentEvent().nextEvents[0].nextEvents[0].nextEvents[0]);
        this.mmm = StoryManager.Instance.currentStory;
        this.SetupProblem();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                print("Correct!");
                StoryManager.Instance.SetProblemSolved(true);
            }
            else
            {
                print("Wrong!");
                StoryManager.Instance.SetProblemSolved(false);
            }
            SceneManager.LoadScene("PathProgressionScene");
        }
    }

    private void SetupProblem()
    {
        this.answerInterface = StoryManager.Instance.GetCurrentAnswerBehaviour(this.transform);
        this.answerInterface.SetupAnswer(StoryManager.Instance.GetCurrentAnswerData());
        this.questionInterface = StoryManager.Instance.GetCurrentQuestionBehaviour(this.transform);
        this.questionInterface.SetupQuestion(StoryManager.Instance.GetCurrentQuestionData());
        this.setupDone = true;
    }
}
