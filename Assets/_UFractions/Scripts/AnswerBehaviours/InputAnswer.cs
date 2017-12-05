using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputAnswer : AnswerBehaviour
{
    public class InputAnswerData : AnswerData
    {
        public List<string> answers;
        public List<ARObjectType> arObjects;

        public InputAnswerData(List<string> answers, List<ARObjectType> arObjects)
        {
            this.answers = answers;
            this.arObjects = arObjects;
        }
    }

    public List<string> answerStrings;
    public InputField answerBox;

    // Use this for initialization
    void Start()
    {
        this.Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Setup function for InputBox.
    /// </summary>
    /// <param name="ans">String, correct answer.</param>
    /// <param name="desc">Text to be displayed in the box.</param>
    public override void SetupAnswer(AnswerData answerData)
    {
        InputAnswerData data = (InputAnswerData)answerData;

        this.answerStrings = data.answers;

        float y = 1.5f;

        foreach (ARObjectType a in data.arObjects)
        {
            InterfaceFactory.GetInstance().SpawnARObject(a, InterfaceFactory.GetInstance().transform, new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), y, UnityEngine.Random.Range(-5.0f, 5.0f)));
            y += 1.5f;
        }
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
