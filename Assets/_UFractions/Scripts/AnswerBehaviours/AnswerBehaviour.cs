using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The interface class used for problems answer part.
/// To create a new AnswerBehaviour create a class that inherit from this class and overrides all of its virtual methods. 
/// </summary>
public class AnswerBehaviour : MonoBehaviour
{
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
    public virtual void SetupAnswer(Story.AnswerStructure answer)
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
