using System;
using UnityEngine;

/// <summary>
/// The interface class used for problems answer part.
/// To create a new AnswerBehaviour create a class that inherit from this class and overrides all of its virtual methods. 
/// </summary>
public class AnswerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Every AnswerBehaviour needs to add thier own Enum AnswerType and set thier type to it.
    /// </summary>
    [Serializable] public enum AnswerType
    {
        None,
        Choices,
        ARSelection,
        Input
    }

    /// <summary>
    /// Every AnswerBehaviour interface needs to implement thier own interface of AnswerData for data managment.
    /// </summary>
    [Serializable] public class AnswerData
    {

    }

    /// <summary>
    /// Interfaces of AnswerBehaviour needs to set this to thier enum type.
    /// </summary>
    public AnswerType answerType;

    /// <summary>
    /// Will be used by the problem to show the AnswerBehaviour interface.
    /// </summary>
    public virtual void Show()
    {

    }

    /// <summary>
    /// Will be used by the problem to hide the AnswerBehaviour interface.
    /// </summary>
    public virtual void Hide()
    {

    }

    /// <summary>
    /// Will be used by the problem to setup the AnswerBehaviour interface.
    /// </summary>
    /// <param name="answer">The AnswerStructure to use to setup the problem.</param>
    public virtual void SetupAnswer(AnswerData answer)
    {
    
    }

    /// <summary>
    /// Will be used by the problem to check if the answer is correct.
    /// </summary>
    /// <returns>Returns true if the answer was correct and false otherwise.</returns>
    public virtual bool GetResult()
    {
        return false;
    }
}
