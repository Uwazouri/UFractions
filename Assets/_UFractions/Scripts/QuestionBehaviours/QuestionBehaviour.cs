using System;
using UnityEngine;

/// <summary>
/// The interface class used for problems question part.
/// To create a new QuestionBehaviour create a class that inherit from this class and overrides all of its virtual methods. 
/// </summary>
public class QuestionBehaviour : MonoBehaviour
{
    /// <summary>
    /// Every QuestionBehaviour needs to add thier own Enum QuestionType and set thier type to it.
    /// </summary>
    [Serializable] public enum QuestionType
    {
        None,
        TextBox,
        Video
    }

    /// <summary>
    /// Every QuestionBehaviour interface needs to implement thier own interface of QuestionData for data managment.
    /// </summary>
    [Serializable] public class QuestionData
    {

    }

    /// <summary>
    /// Interfaces of QuestionBehaviour needs to set this to thier enum type.
    /// </summary>
    public QuestionType questionType;

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
    public virtual void SetupQuestion(QuestionData question)
    {

    }
}
