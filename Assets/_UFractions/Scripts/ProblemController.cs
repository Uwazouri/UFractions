using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the problem scene by using the StoryManager to setup the current events problems interfaces.
/// Note: Needs to have a path selected at least before being able to function.
/// Flow: Displays problem to user and when user answers determines if the problem was solved or not and loads path progression scene.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
public class ProblemController : MonoBehaviour
{
    private QuestionBehaviour questionInterface;
    private AnswerBehaviour answerInterface;
    private StoryManager storyManager;
    private bool setupDone = false;
    private bool answerOn = false;
    private bool questionOn = false;
    private bool problemHasNoAnswer = false;

	/// <summary>
    /// Save instance of storymanager for quicker access and setup problem on start.
    /// </summary>
	void Start ()
    {
        this.storyManager = StoryManager.Instance;
        this.SetupProblem();
    }

    /// <summary>
    /// Toggles the answer part on or off.
    /// </summary>
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

    /// <summary>
    /// Toggles the question part on or off.
    /// </summary>
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

    /// <summary>
    /// Check if answer is correct or not and set lastproblemsolved in storymanager and loads pathprogression scene.
    /// </summary>
    public void CommitAnswer()
    {
        if (this.setupDone)
        {
            if (!this.problemHasNoAnswer)
            {
                if (this.answerInterface.GetResult())
                {
                    print("Correct!");
                    this.storyManager.SetProblemSolved(true);
                }
                else
                {
                    print("Wrong!");
                    this.storyManager.SetProblemSolved(false);
                }
                SceneManager.LoadScene("PathProgressionScene");
            }
            else
            {
                this.storyManager.SetProblemSolved(true);
                SceneManager.LoadScene("PathProgressionScene");
            }
        }
    }

    /// <summary>
    /// Use to setup current problem.
    /// </summary>
    private void SetupProblem()
    {
        this.SetupProblem(this.storyManager.GetCurrentProblem().problemID);
    }

    /// <summary>
    /// Use to setup a specific problem.
    /// </summary>
    /// <param name="problemID"></param>
    private void SetupProblem(uint problemID)
    {
        this.questionInterface = this.storyManager.GetQuestionBehaviour(this.transform, problemID);
        this.questionInterface.SetupQuestion(this.storyManager.GetCurrentQuestionData(problemID));
        if (this.storyManager.GetProblem(problemID).HasAnswer())
        {
            this.answerInterface = this.storyManager.GetAnswerBehaviour(this.transform, problemID);
            this.answerInterface.SetupAnswer(this.storyManager.GetAnswerData(problemID));
        }
        else
        {
            this.problemHasNoAnswer = true;
        }
        this.setupDone = true;
    }
}
