using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Answer interface that uses simple input field to answer a question.
/// </summary>
public class InputAnswer : AnswerBehaviour
{
    /// <summary>
    /// Data interface for InputAnswer.
    /// </summary>
    [Serializable] public class InputAnswerData : AnswerData
    {
        public List<string> answers;

        /// <summary>
        /// Constructor sets the data.
        /// </summary>
        /// <param name="answers">A list of accepted answers.</param>
        public InputAnswerData(List<string> answers)
        {
            this.answers = answers;
        }
    }

    public List<string> answerStrings;
    public InputField answerBox;

    /// <summary>
    /// Hides answer part on start.
    /// </summary>
    void Start()
    {
        this.Hide();
    }

    /// <summary>
    /// Setup function for InputBox.
    /// </summary>
    /// <param name="answerData">AnswerData for problem.</param>
    public override void SetupAnswer(AnswerData answerData)
    {
        InputAnswerData data = (InputAnswerData)answerData;

        this.answerStrings = data.answers;
    }

    /// <summary>
    /// Check if correct answer is given.
    /// </summary>
    /// <returns>Returns true if correct answer, otherwise false.</returns>
    public override bool GetResult()
    {
        foreach (string s in this.answerStrings)
        {
            if (s.ToLower() == answerBox.text.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Show InputBox.
    /// </summary>
    public override void Show()
    {
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// Hide InputBox.
    /// </summary>
    public override void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
