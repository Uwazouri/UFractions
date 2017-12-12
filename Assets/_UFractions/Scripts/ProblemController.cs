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
    private bool problemHasNoAnswer = false;

	// Use this for initialization
	void Start ()
    {
        //StoryManager.Instance.CreateDebugStory("FakeStory");
        //StoryManager.Instance.SetCurrentStory(StoryManager.Instance.GetLocalStories()[0]);
        //StoryManager.Instance.SetCurrentPath(StoryManager.Instance.GetAllPaths()[0]);
        //StoryManager.Instance.SetCurrentEvent(StoryManager.Instance.GetCurrentEvent().nextEvents[0].nextEvents[0].nextEvents[0]);
        //this.mmm = StoryManager.Instance.currentStory;
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
            if (!this.problemHasNoAnswer)
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
                if (!this.problemHasNoAnswer)
                {
                    this.answerOn = false;
                    this.answerInterface.Hide();
                }
                this.questionInterface.Show();
                this.questionOn = true;
            }

        }
    }

    public void CommitAnswer()
    {
        if (this.setupDone)
        {
            if (!this.problemHasNoAnswer)
            {
                if (this.answerInterface.GetResult())
                {
                    print("Correct!");
                    StoryManager.Instance.SetProblemSolved(true);
                }
                else
                {
                    print("Wrong!");
                    StoryManager.Instance.SetProblemSolved(false);
                }
                SceneManager.LoadSceneAsync("PathProgressionScene", LoadSceneMode.Single);
            }
            else
            {
                StoryManager.Instance.SetProblemSolved(true);
                SceneManager.LoadSceneAsync("PathProgressionScene", LoadSceneMode.Single);
            }
        }
    }

    private void SetupProblem()
    {
        this.SetupProblem(StoryManager.Instance.GetCurrentProblem().problemID);
    }

    private void SetupProblem(uint problemID)
    {
        this.questionInterface = StoryManager.Instance.GetCurrentQuestionBehaviour(this.transform, problemID);
        this.questionInterface.SetupQuestion(StoryManager.Instance.GetCurrentQuestionData(problemID));
        if (StoryManager.Instance.GetProblem(problemID).HasAnswer())
        {
            this.answerInterface = StoryManager.Instance.GetCurrentAnswerBehaviour(this.transform, problemID);
            this.answerInterface.SetupAnswer(StoryManager.Instance.GetCurrentAnswerData(problemID));
        }
        else
        {
            this.problemHasNoAnswer = true;
        }
        this.setupDone = true;
    }
}
