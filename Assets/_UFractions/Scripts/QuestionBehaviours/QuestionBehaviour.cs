using UnityEngine;

/// <summary>
/// The interface class used for problems question part.
/// To create a new QuestionBehaviour create a class that inherit from this class and overrides all of its virtual methods. 
/// </summary>
public class QuestionBehaviour : MonoBehaviour
{
    /// <summary>
    /// Will be used by the problem to show the QuestionBehaviour interface.
    /// </summary>
    public virtual void Show()
    {

    }

    /// <summary>
    /// Will be used by the problem to hide the QuestionBehaviour interface.
    /// </summary>
    public virtual void Hide()
    {

    }

    /// <summary>
    /// Will be used by the problem to setup the QuestionBehaviour interface.
    /// </summary>
    /// <param name="question">The QuestionBehaviour to use to setup the question.</param>
    public virtual void SetupQuestion(Story.QuestionStructure question)
    {

    }
}
